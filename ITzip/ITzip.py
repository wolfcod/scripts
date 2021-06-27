#!/usr/bin/env python3
import argparse
import os
import sys
from os.path import basename
from os import path
from zipfile import ZipFile
import datetime

# parse_args
def parse_args(argv):
    argparser = argparse.ArgumentParser(epilog='ITzip')
    argparser.add_argument('-i', '--input', help='Input directory')
    argparser.add_argument('-o', '--output', help='Output directory')
    argparser.add_argument('-s', '--seq', help='Primo elemento sequenza')
    argparser.add_argument('-d', '--data', help='Filtro data')
    argparser.add_argument('-m', '--max', help='max file per zip (default 10)')
    #print("parse_args parsing =>", argv)
    args = argparser.parse_args(argv)
    print("Args parsed are: =>", args)
    args_dict = {
        'input': args.input,
        'output': args.output,
        'seq': args.seq,
        'data': args.data,
        'max': args.max
    }

    # Verifica gli input forniti...
    if args.input is None:
        print('Errore: input directory richiesto.')
        sys.exit(1)
    
    if args.output is None:
        print('Errore: output directory richiesto.')
        sys.exit(1)
    
    if args.max is not None:
        try:
            int(arg.max)
        except:
            print('Errore: --max numero!')
            sys.exit(1)
        
    if args.seq is not None:
        try:
            int(args.seq)
        except:
            print('Errore: --seq numero!')
            sys.exit(1)

    if args.data is not None:
        try:
            datetime.datetime.strptime(args.data, '%Y-%m-%d')
        except:
            print('Errore: --data formato YYYY-MM-DD')
            sys.exit(1)    
    return args_dict

def createZipName(zipDir, seqNo):
    #'IT00000000000_00001'
    fileName = 'IT00000000000_{:05d}.zip'.format(seqNo)
    return os.path.join(zipDir, fileName)

def validateName(filename):
    filename = basename(filename)
    if filename.find('IT') == 0:
        return filename
    
    tag = filename.find('_')

    newname = 'IT00000000000_' + filename[tag+1:]
    print('File %s => %s' %(filename, newname))
    return newname

# check filename... discard RC
def check(filename):
    if filename.find('_RC_') >= 0:
        #print('File %s scartato' %(filename))
        return False
    
    return True

def dataFattura(filename):
    f = open(filename)
    content = f.read()
    f.close()
    #doc = minidom.parseString(content)
    #data = doc.getElementsByTagName('Data')

    tagStart = content.find('<Data>')
    tagEnd = content.find('</Data>')

    data = content[tagStart+6:tagEnd]
    #print('Fattura del %s' %(data))
    return data

def inputFolder(dirName):
    content = {}
    for root, dirs, files in os.walk(dirName):
        for filename in files:
            writable = check(basename(filename))

            if writable:
                fullName = os.path.join(root, filename)
                content[fullName] = dataFattura(fullName)
    
    return content

# function to get unique values
def unique(list1):
 
    # intilize a null list
    unique_list = []
     
    # traverse for all elements
    for x in list1:
        # check if exists in unique_list or not
        if x not in unique_list:
            unique_list.append(x)
    return unique_list

# crea il file IT.......SEQNO.zip
def creaArchivio(zipDir, seqNo):
    zipName = createZipName(zipDir, seqNo)
    print('Creazione archivio %s' %(zipName))
    z = ZipFile(zipName, 'w')
    return z
    
def ITzip(args):
    dirName = args['input']
    zipDir = args['output']
    seqNo = 1

    validContent = inputFolder(dirName)
    # campo <Data>xxx</Data> dall'xml.. ordinato in crescente
    data_list = sorted(unique(list(validContent.values())))
 
    # number of files per zip
    maxElements = 10
    elements = 0

    endDate = '9999-12-31'
    if args['data'] is not None:
        endDate = args['data']

    if args['max'] is not None:
        maxElements = int(args['max'])

    zipArchive = creaArchivio(zipDir, seqNo)

    # per ogni data presente nella lista..
    for data in data_list:
        if (data < endDate):
            # Le fatture di questa data sono da prendere in considerazione...
            print('Fatture del %s' %(data))
            for x in validContent:
                if validContent[x] == data:
                    # fattura corrispondente
                    fileName = x
                    toZipName = validateName(fileName)

                    print('Aggiungo %s [%s]' %(fileName, validContent[x]))
                    zipArchive.write(fileName, toZipName)
                    elements += 1

                    if elements == 10:
                        # reset di questo archivio
                        zipArchive.close()
                        seqNo += 1
                        zipArchive = creaArchivio(zipDir, seqNo)
                        elements = 0
        else:
            print('Data scartata %s' %(data))

    return seqNo

def main(argv=sys.argv[1:]):
    args = parse_args(argv)
    if args is None:
        return 1

    seqNo = ITzip(args)
    print(seqNo)

if __name__ == "__main__":
    main()
