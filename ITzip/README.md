# ITzip

Script python per creare archivi zip da caricare sul sito dell'AE.
Sintassi:  
   `ITzip.py`  
   `--input cartellainput`: Cartella dove sono contenuti gli xml  
   `--output cartellazip`: Cartella dove contenere gli zip  
   `--data YYYY-MM-DD`: Le fatture da questa data vengono scartate  
   `--max`: Numero max di file per zip.. default 10  
   `--seq`: Numero sequenza.. utile se si vogliono salvare i diversi zip. Diversamente, da 1.  
   
I file contenenti `SMxxxx` sono rinominati in `ITxxxxxx_tag.estensione`  
I file contenenti la dicitura `_RC` sono esclusi  

Richiede python 2.7.x => Testato su mac e windows.

