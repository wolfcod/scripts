/*
  *@fileoverview This is Mio Dodge API,you can use it get device infomation if you want and
  *   it can be used to process Dodge business logic.
  *@copyright Copyright @2012 MBG SWRDC RDD3 SW2
  *@company Mitac
  *@time 2013.03.15
  *
  *@author show.liao
  *@email show.liao@mic.com.tw
  *@version ver 3.3.0.016
*/

/**
 * -PROPERTY-
 * @description Declare common Mitac Device Sync Plug-ins object.
 * @param PluginTmpt Mitac Device Sync Plug-ins object
 * @type Object
 * @public
*/
var PluginTmpt = null;

var GSPlugin = null;

var initialStatus = false;
/**
 * -PROPERTY-
 * @description Declare common country abbreviation mapping to country name object.
 * @param hashtable hashTableObject
 * @type hashtable
 * @example assert(countryTableInit().get("EUR") == "EUROPE");
 * @public
*/
var hashtable = countryTableInit();

/**
 * -PROPERTY-
 * @description Declare common ErrorCode object.
 * @param hashtable hashTableObject
 * @type hashtable
 * @public
*/
var errorList = errorCode();

/**
 * -PROPERTY-
 * @description Declare common Maps data iso file name.
 * @param isoName propertyType
 * @type String
 * @public
 * @since ver1.0
*/
//var isoName="";

var isoUrl="";
var mMapsDoc;
var delMap;
var delMapCount;
var addMap;
var addMapCount;

var deviceType = "";

String.prototype.trim=function(){return this.replace(/^\s\s*/, '').replace(/\s\s*$/, '');};
function parseSlashes(mPath) {
	try {
		if (mPath != NaN || mPath != undefined) {			
	 		var ua = navigator.userAgent.toLowerCase(), s, o = {};
			var rePath = "";
			var processPath = String(mPath);
			if (platformStr === 'mac') {
				processPath = processPath.replace(/\\\\/g,"/");
				processPath = processPath.replace(/\\/g,"/");
				rePath = processPath;
			}
			else if (platformStr === 'win') {				

				if ((s=ua.match(/chrome\/([\d.]+)/))) {
					processPath = processPath.replace(/\\\\/g,"\\");
					processPath = processPath.replace(/\\/g,"\\");
					rePath = processPath;
				}  else {
					processPath = processPath.replace(/\\/g,"\\");
					rePath = processPath;
				}
			}
			
		} else {
			rePath =  mPath;
		}
	} catch (err) {

	}
	return rePath;
}
function checkEnd(str)
{
    var platform = window.navigator.platform;
    var ua = navigator.userAgent.toLowerCase(), s, o = {};
    if (platform.indexOf("Mac") === - 1)
    {
        if ( -1 == str.indexOf('\\', (str.length -1)))
            return str + '\\';
        else
            return str;
    } else if ((platform === 'Win32') && (s=ua.match(/chrome\/([\d.]+)/))) {
        if ( -1 == str.indexOf('\\', str.length)) {
          return str + '\\';
        }
        else {
          return str;
        }
    } else {
        return str;
    }
}

function replaceSlash(str)
{
    var platform = window.navigator.platform;
    if (platform.indexOf("Mac") === - 1) {
	    var tempPath = str.replace(/\\\\/g, '\\');   //\\
        tempPath = tempPath.replace(/\\\\\\/g, '\\');   //\\\
        return tempPath = tempPath.replace(/\\/g, '\\\\');   //\\
	}
    else
        return str;
}
/** 
 * Create new DeviceDisplay Class.
 * @class Get device information
 */
function DeviceControl() 
{
	/** 
	* Get device kernel image firmware version information.
	* @return {String} Device firmware version string
	* @example alert(new DeviceControl().getFirmwareVersion()); 
	*/
    DeviceControl.prototype.getFirmwareVersion = function() { 
		var firewareVersion = "";
		var firmware = "";
		if (deviceType == "Mio") {
			var kernel = PluginTmpt.MITAC_ReadXmlFileContent(parseSlashes(PluginTmpt.MITAC_GetSystemPath(0)+"device.xml"),"device/kernel/version");
	        firmware = PluginTmpt.MITAC_ReadXmlFileContent(parseSlashes(PluginTmpt.MITAC_GetSystemPath(0)+"device.xml"),"device/bootloader/version");
	        firewareVersion = firmware + "(" + kernel + ")";
		}
		else if(deviceType == "GS") {
			firewareVersion = GSPlugin.CYC100_GetFirmwareVersion();
		}
		else {
			firewareVersion = PluginTmpt.MITAC_ReadXmlFileContent(parseSlashes(PluginTmpt.MITAC_GetSystemPath(0)+"device.xml"),"device/firmware/version");
			if (firewareVersion == "none") {
				var kernel = PluginTmpt.MITAC_ReadXmlFileContent(parseSlashes(PluginTmpt.MITAC_GetSystemPath(0)+"device.xml"),"device/kernel/version");
				firmware = PluginTmpt.MITAC_ReadXmlFileContent(parseSlashes(PluginTmpt.MITAC_GetSystemPath(0)+"device.xml"),"device/bootloader/version");
				firewareVersion = firmware + "(" + kernel + ")";
			}
		}
        return firewareVersion;
    };
	
	/** 
	* Get device GPS First Use Date information.
	* @return {String} Device GPS First Use Date string
	* @example alert(new DeviceControl().getFirstUseDate()); 
	*/
    DeviceControl.prototype.getFirstUseDate = function() { 
        var fud = PluginTmpt.MITAC_ReadXmlFileContent(parseSlashes(PluginTmpt.MITAC_GetSystemPath(0)+"device.xml"),"device/application/firstuse");
        if(fud == "none")
        {
            fud = "";
        }
        return fud;
    };

	/** 
	*Get device marketing name information.
	* @return {String} Device marketing name string
	* @example alert(new DeviceControl().getHardwareDisPlayName()); 
	*/ 
    DeviceControl.prototype.getHardwareDisplayName = function() {
        var display = "";

		if (deviceType == "Mio") {
			display = PluginTmpt.MITAC_ReadXmlFileContent(parseSlashes(PluginTmpt.MITAC_GetSystemPath(0)+"device.xml"),"device/hardware/displayname");
		}
		else if(deviceType == "GS") {
			display = GSPlugin.CYC100_GetDeviceID();
		}
		else {
			display = PluginTmpt.MITAC_ReadXmlFileContent(parseSlashes(PluginTmpt.MITAC_GetSystemPath(0)+"device.xml"),"device/hardware/displayname");
		}
		return display;
	};

	/** 
	* Get device SKU ID information.
	* @return {String} Device SKU ID string
	* @example alert(new DeviceControl().getSKUID()); 
	*/
    DeviceControl.prototype.getSKUID = function() { 
		var skuName = "";
		
		if (deviceType == "Mio") {
			skuName = PluginTmpt.MITAC_ReadXmlFileContent(parseSlashes(String(PluginTmpt.MITAC_GetSystemPath(0)+"device.xml")),"device/sku/skuid");
		}
		else if(deviceType == "GS") {
			skuName = GSPlugin.CYC100_GetSKUID();
		}
        return skuName;
    };

	
	/** 
	* Get device system drive flash free available space information, unit: Byte.
	* @return {String} Device system drive flash free available space information, unit: Byte
	* @example alert(new DeviceControl().getSystemDriveFree_Flash()); 
	*/
    DeviceControl.prototype.getSystemDriveFreeFlash = function() {
	    var result;
		
		if (deviceType == "Mio") {
			result = PluginTmpt.MITAC_GetDeviceFreeMem("DEVICE");
		}
		else if(deviceType == "GS") {
			result = "";
		}		
		
		if (result == "undefined") {
			result = "";
		}
        return result;
    };
	
	/** 
	* Get device data drive flash free available space information, unit: Byte.
	* @return {String} Device data drive flash free available space information, unit: Byte
	* @example alert(new DeviceControl().getDataDriveFree_Flash()); 
	*/
    DeviceControl.prototype.getDataDriveFreeFlash = function() {
        var result;
		
		if (deviceType == "Mio") {
			result = PluginTmpt.MITAC_GetDeviceFreeMem("DATA");
		}
		else if(deviceType == "GS") {
			result = GSPlugin.CYC100_getDeviceFreePoints();
			if (result == 600)
			    result = 0;
		}
		
		if (result == "undefined") {
			result = "";
		}
        return result;
    };
	
	/** 
	* Get device UUID information.
	* @return {String} Device UUID string
	* @example alert(new DeviceControl().getUUID()); 
	*/
    DeviceControl.prototype.getUUID = function() { 
        var uuid = "";
		
		if (deviceType == "Mio") {
			uuid = PluginTmpt.MITAC_ReadXmlFileContent(parseSlashes(PluginTmpt.MITAC_GetSystemPath(0)+ "device.xml"),"device/hardware/id");
		}
		else if(deviceType == "GS") {
			uuid = GSPlugin.CYC100_GetUUID();			
		}
        return uuid;
    };
	
	DeviceControl.prototype.getSHA512 = function(path) { 
        				
        return PluginTmpt.MITAC_GetSHA512(path);
    };
	
    DeviceControl.prototype.initialPlugin = function(mPluginObj,usbConnectListener) {
		PluginTmpt = mPluginObj;
				
		var lock = PluginTmpt.MITAC_UnlockPlugin('127.0.0.1','a34af967dfe6cce82755c48b310f554b');
		var MioResult = PluginTmpt.MITAC_SetDeviceConnect(usbConnectListener);		
		        				      
        if(MioResult === 0 && deviceType != "GS")
        {
			deviceType = "Mio";            
			return true;
        }
        else
            return false;
    };
    DeviceControl.prototype.initialCYC100Plugin = function(GPluginObj,usbConnectListener) {		
		GSPlugin = GPluginObj;
			                                
		var platform = window.navigator.platform;
		var GSresult;
		GSresult = GSPlugin.CYC100_SetDeviceConnect(usbConnectListener);
				
        if (deviceType != "Mio" && GSresult === 1)
		{
			return true;
		}
		else{
			return false;
		}
    };

	/** 
	* Check specific file exists in device or not.
	* @return {Boolean} Check specific file exists in device
	* @param {String} path Source file path
	* @example alert(new DeviceControl().checkFileExist('c:\\maps.xml')); 
	*/
    DeviceControl.prototype.checkFileExist = function(path) { 
		var exist = "";
		path = parseSlashes(path);
		if (deviceType == "Mio") {		  
			//exist = PluginTmpt.MITAC_CheckFileExist(path);
			exist = PluginTmpt.MITAC_CheckFileExist(path);			
		}
		else if(deviceType == "GS") {
			if(path.split(":")[0].toUpperCase() == "DEVICE") {
				var emptyPath = "";
				exist = GSPlugin.CYC100_checkFileExist(path.split(":")[1], 0, emptyPath);                			
			}
			else {
				exist = PluginTmpt.MITAC_CheckFileExist(path);
			}
		}
		else {
			if (path.split(":")[0].toUpperCase() == "DEVICE") {
				exist = "";
			}
			else {
				exist = PluginTmpt.MITAC_CheckFileExist(path);
			}
		}
            
        if(exist == errorList.get("E_MITAC_PLUGIN_OK")) {
    	    return true;
        }
		else {
    	    return false;
        }
    };

	/** 
	* Check specific file size.
	* @return {Number} File size, unit: Byte
	* @param {String} path File path
	* @example alert(new DeviceControl().checkFileSize('c:\\mapsfile\\maps.xml'));
	*/
    DeviceControl.prototype.checkFileSize = function(path) { 
    	path = parseSlashes(path);
		var fileSize;
		if (deviceType == "Mio") {
			fileSize = PluginTmpt.MITAC_GetFileSize(path);
		}
		else if(deviceType == "GS") {
			fileSize = GSPlugin.CYC100_checkFilePoints(path);			
		}
		
		if (fileSize == undefined) {
			fileSize = "";
		}
		fileSize = parseInt(fileSize);
        return fileSize;
    };

    DeviceControl.prototype.getSystemDiskFreeFlash = function(disk) {    
		disk = parseSlashes(disk);

        return PluginTmpt.MITAC_GetDeviceFreeMem(disk);
    };	
	/** 
	* Copy file to device method.
	* @return {Boolean} Copy file method success or fail.
	* @param {String} from Source file path
	* @param {String} to Target directory path
	* @param {Object} listener - asynchronous file copy listener object  (optional)
	* @example Without listener: alert(new DeviceControl().copyFileSync('c:\\mapsfile\\test.xml','c:\\map'));
	*                    With listener: alert(new DeviceControl().copyFileSync('c:\\mapsfile\\test.xml','c:\\map', listenerObject));
	*/
    DeviceControl.prototype.copyFileSync = function(from, to, listener) { 
    	from = parseSlashes(from);

		to = parseSlashes(to);

		var result = "";
		
		if (deviceType == "Mio") {
			if (listener != undefined) {
				result = PluginTmpt.MITAC_CopyFile(from, to, listener);
			}
			else {
				result = PluginTmpt.MITAC_CopyFile(from, to, null);
			}
		}
		else if(deviceType == "GS") {
			if(from.split(":")[0].toUpperCase() == "DEVICE") {
				result = GSPlugin.CYC100_saveFile(from.split(":")[1], listener);				
			}
			else if(to.split(":")[0].toUpperCase() == "DEVICE") {
				result = GSPlugin.CYC100_copyFile(from, listener);
			}
			else {
				result = PluginTmpt.MITAC_CopyFile(from, to, null);
			}
		}
		
        if(result == errorList.get("E_MITAC_PLUGIN_OK"))
        {
            return true;
        }else
        {
            return false;
        }
    };
	
	/** 
	* Create new directory.
	* @return {Boolean} Create new directory method success or fail.
	* @param {String} path New directory path
	* @example alert(new DeviceControl().createDirectory('c:\\mapsfile\\')); 
	*/
    DeviceControl.prototype.createDirectory = function(path) { 
    	path = parseSlashes(path);
        var result = PluginTmpt.MITAC_CreateDirectory(path);
        if(result == errorList.get("E_MITAC_PLUGIN_OK"))
        {
            return true;
        }else
        {
            return false;
        }
    };
	
	/** 
	* Delete specific directory.
	* @return {Boolean} Delete directory method success or fail.
	* @param {String} directoryPath Delete directory path
	* @example alert(new DeviceControl().deleteDirectory('c:\\mapsfile\\')); 
	*/
    DeviceControl.prototype.deleteDirectory = function(directoryPath) { 
    	directoryPath = parseSlashes(directoryPath);
        var result = PluginTmpt.MITAC_DeleteDirectory(directoryPath);
        if(result == errorList.get("E_MITAC_PLUGIN_OK"))
        {
            return true;
        }else
        {
            return false;
        }
    };
	
	/** 
	* Load file.
	* @return {String} Load file success or fail.
	* @param {String} path File path
	* @example alert(new DeviceControl().loadFile('c:\\mapsfile\\maps.xml')); 
	*/
    DeviceControl.prototype.loadFile = function(path) { 
    	path = parseSlashes(path);
       	var result = "";
       	if (PluginTmpt.MITAC_CheckFileExist(path) == 0) {
			result = PluginTmpt.MITAC_LoadFile(path);  
       	} else {

       	}	
        return result;
    };

	/**
	* Load file List
	*@return{Array}
	*@param{String} Directory path
	* @example alert(new DeviceControl().loadFileList('c:\\mapsfile\\'));
	*/
	DeviceControl.prototype.loadFileList = function(path) {
    	path = parseSlashes(path);
		var result = "";
		
		if (deviceType == "Mio") {
		    path = checkEnd(path);
			result = PluginTmpt.MITAC_LoadFileList(path);
		}
		else if(deviceType == "GS") {
				if (path.split(":")[0].toUpperCase() == "DEVICE") {				    
					result = GSPlugin.CYC100_LoadFileList(0, ' ');
				}
				else {
					result = GSPlugin.CYC100_LoadFileList(2, path);
				}
		}
		else {
			if (path.split(":")[0].toUpperCase() == "DEVICE") {
				result = "";
			}
			else {
			    path = checkEnd(path);
				result = PluginTmpt.MITAC_LoadFileList(path);
			}
		}		
		var fileListArr = result.split('|');
		return fileListArr;
	};
	
	/** 
 	  * Save string into specific file.
 	  * @return {Boolean} Save string method success or fail.
 	  * @param {String} str Source file path
 	  * @param {String} file Target path to save as a file
 	  * @example alert(new DeviceControl().saveStringAsFile('testtest','c:\\mapsfile\\test.xml')); 
 	  */
    DeviceControl.prototype.saveStringToFile = function(str,file) {
    	file = parseSlashes(file);

	var result = "";
	result = PluginTmpt.MITAC_SaveFile(str,file);
		
        if (result == errorList.get("E_MITAC_PLUGIN_OK")) {
            return true;
        }
		else {
            return false;
        }
    };
	
	/**
	*Get device system drive
	*@return{String} Format ex: F:\
	*@param{}
	* @example alert(new DeviceControl().deviceSystemDrive();
	*/
	DeviceControl.prototype.getSystemDrive = function() { 
	    var result = "";
		
		if (deviceType == "Mio") {
			result = parseSlashes(PluginTmpt.MITAC_GetDeviceDrive("S"));
		}
		else if(deviceType == "GS") {
			result = GSPlugin.CYC100_GetSystemPath();
		}
		else {
			result = parseSlashes(PluginTmpt.MITAC_GetSystemPath(0));
		}
        return result;
    };

	/**
	*Get device data drive
	*@return{String} format ex: F:\
	*@param{}
	* @example alert(new DeviceControl().deviceDataDrive();
	*/
	DeviceControl.prototype.getDataDrive = function() { 
		var result = "";
		if (deviceType == "Mio") {
			result = parseSlashes(PluginTmpt.MITAC_GetDeviceDrive("D"));
		}
		else if(deviceType == "GS") {
			result = "DEVICE:";
        }
        return result;
    };
	
	/** 
	* Delete specific file.
	* @return {Boolean} Delete file method success or fail.
	* @param {String} filePath Delete file path
	* @example alert(new DeviceControl().deleteFile('c:\\mapsfile\\test.xml')); 
	*/
    DeviceControl.prototype.deleteFile = function(filePath) { 
    	filePath = parseSlashes(filePath);

		var result = 24;
        filePath = filePath.toUpperCase();		
		if (deviceType == "Mio") {		    
			result = PluginTmpt.MITAC_DeleteFile(filePath);
		}
		else if(deviceType == "GS") {
		    var emptyPath = "";
			if(filePath.split(":")[0] == "DEVICE") {            			
			result = GSPlugin.CYC100_deleteDrvFile(filePath.split(":")[1].toLowerCase(), 0, emptyPath);
			}
			else
			{
			     emptyPath = GSPlugin.CYC100_GetSystemPath().toUpperCase();			     
			     if (-1 == filePath.indexOf(emptyPath))
			         result = 24;
			     else
                 {
                    var mPos = filePath.lastIndexOf('\\');
                    if (mPos == filePath.length)
                        result = 24;
                    else
                    {                           
                        var str = filePath.substring(0, mPos+1);                       
                        var fileName = filePath.substring(mPos+1, filePath.length);                         
                        result = GSPlugin.CYC100_deleteDrvFile(fileName, 2, str);
                    }                           
                 }
            }
		}
		return result;
    };
    
    DeviceControl.prototype.moveFile = function(srcPath, destPath, listener) { 
    	srcPath = parseSlashes(srcPath);
    	destPath = parseSlashes(destPath);

		var result = PluginTmpt.MITAC_MoveFile(srcPath, destPath, listener);        
		if(result == errorList.get("E_MITAC_PLUGIN_OK"))
        {
            return true;
        }
        return false;                
    };
    
    DeviceControl.prototype.checkLicense = function(skuId, uuId, str, srcPath) {    
    	srcPath = parseSlashes(srcPath);

		var result = PluginTmpt.MITAC_CheckLicense(skuId, uuId, str, srcPath);        
		if(result == errorList.get("E_MITAC_PLUGIN_OK"))
        {
            return true;
        }
        return false;                
    };
    
    DeviceControl.prototype.getSystemDiskFreeFlash = function(disk) {
    	disk = parseSlashes(disk);

	    var result;
		if (deviceType == "Mio") {
			result = PluginTmpt.MITAC_GetDeviceFreeMem(disk);
		}
		else if(deviceType == "GS") {
			result = "";
		}
		
		if (result == "undefined") {
			result = "";
		}
        return result;
    };
    
    DeviceControl.prototype.releaseConnThread = function() { 
        PluginTmpt.MITAC_ReleaseConnThread(0);        
    };	

    /** 
	* Get temporality directory path(use for extract iso file or download caching). 
	* @return {String} Temporality directory path
	* @example alert(new ExtraLibs().getTempFolder()); 
	*/
    DeviceControl.prototype.getTempFolder = function() {
        var filePath = "";
		
		if (deviceType == "Mio") {
			filePath = parseSlashes(PluginTmpt.MITAC_GetSystemPath(0));
		}
		else if(deviceType == "GS") {
			filePath = GSPlugin.CYC100_GetSystemPath();
		}
		else {
			filePath = parseSlashes(PluginTmpt.MITAC_GetSystemPath(0));
		}
		
        return filePath;
    };

	/** 
	* Get current browser type(use for detect the browser type).
	* @return {array} array included browser type, info and version
	* @example 
	var o = new DeviceControl().getBrowserType();
	IE case: alert(o.ie);
	FireFox case: alert(o.ff);
	Chrome case: alert(o.chrome);
	*/
    DeviceControl.prototype.getBrowserType = function() {
        var ua = navigator.userAgent.toLowerCase(), s, o = {};
        if( s=ua.match(/msie ([\d.]+)/) )
        {   
            o.ie = true;
            o.info = "ie";
        } 
        else if( s=ua.match(/firefox\/([\d.]+)/) ) 
        {   
            o.ff = true;
            o.info = "ff";
        } 
        else if( s=ua.match(/chrome\/([\d.]+)/) ) 
        {   
            o.chrome = true;
            o.info = "chrome";
        } 
        else if( s=ua.match(/opera.([\d.]+)/) ) 
        {   
            o.opera = true;
            o.info = "opera";
        } 
        else if( s=ua.match(/version\/([\d.]+).*safari/) ) 
        {   
            o.safari = true;
            o.info = "safari";
        }
	    
        if( s && s[1] ) 
        {   
            o.version = parseFloat( s[1] );
        } else {   
            o.version = 0;
        }   
        o.info = (o.info?o.info:"") + "_" + o.version;
        return o;
    };
	
	/** 
	* Write out Mitac Device Sync Plug-in object tag into HTML page and return Mitac Device Sync Plug-in object.
	* @return {Object} Mitac Device Sync Plug-ins Object
	* @example new DeviceControl().getPluginObject();
	*/
    DeviceControl.prototype.getPluginObject = function() {
		var platform = window.navigator.platform;
        var o = this.getBrowserType();
        
		if (o.ie)
        {
			if (platform  == "Win32" && window.navigator.cpuClass == "x86") {
				document.write('<object id="PluginObject" classid="clsid:BF515938-8FA9-4524-92C4-DA654AE025FE" codebase="mioPlug.dll" style="visibility:hidden;" VIEWASTEXT></object>');
			}
        }
		else if (o.ff)
        {
			    document.write('<EMBED id="PluginObject" type="application/mitac-device-sync" pluginspage="npMioPlugIn.dll" width=0 height=0  style="visibility:hidden;"/>');
        }
		else if (o.chrome)
        {
			document.write('<EMBED id="PluginObject" type="application/mitac-device-sync" width=0 height=0  style="visibility:hidden;"/>');
			
        }
		else if (o.safari)
        {                       
			if(platform.indexOf("Mac") > - 1 ) {
				document.write('<EMBED id="PluginObject" type="application/mitac-device-sync" pluginspage="mioplugin.plugin" width=0 height=0  style="visibility:hidden;"/>');
			}
        }
		
		return document.getElementById('PluginObject');
    };
    
    DeviceControl.prototype.getPluginCyc100Object = function() {
		var platform = window.navigator.platform;
        var o = this.getBrowserType();
		if (o.ie)
        {
			if (platform  == "Win32" && window.navigator.cpuClass == "x86") {				
				document.write('<body><OBJECT ID="MioCyclo_100_Series" WIDTH=0 HEIGHT=0 CLASSID="CLSID:FAEE64B0-2E58-4033-8C1C-EB321AC47883" CODEBASE="ieMioCyclo100PlugIn.dll"></OBJECT></body>');
			}
        }
		else if (o.ff)
        {
			document.write('<EMBED id="MioCyclo_100_Series" type="application/mitac-cyclo100-sync" pluginspage="npMioCyclo100PlugIn.dll" width=0 height=0  style="visibility:hidden;"/>');
        }
		else if (o.chrome)
        {
	        document.write('<EMBED id="MioCyclo_100_Series" type="application/mitac-cyclo100-sync" pluginspage="npMioCyclo100PlugIn.dll" width=0 height=0  style="visibility:hidden;"/>');
        }
		else if (o.safari)
        {
			if(platform.indexOf("Mac") > - 1 ) {
			document.write('<EMBED id="MioCyclo_100_Series" type="application/mitac-cyclo100-sync" pluginspage="npMioCyclo100PlugIn.dll" width=0 height=0  style="visibility:hidden;"/>');
			}
        }
		  
		return document.getElementById('MioCyclo_100_Series'); 
    };
	
    DeviceControl.prototype.download = function(downloadUrl,downloadPath,listener,isBreakPoint) {
		downloadPath = parseSlashes(downloadPath);
		var platform = window.navigator.platform;
		// If Copy this File Please remove below code Line 799 To Line 
		// console.log("<pre>"+DeviceControl.download().toString().replace("<","&lt;")+"</pre>");

		// console.log("MITAC_DownloadFile retrun CODE is '" + PluginTmpt.MITAC_DownloadFile(downloadUrl, downloadPath, listener) + "'");

		if(platform.indexOf("Mac") > - 1 ) {
			result = PluginTmpt.MITAC_DownloadFile(downloadUrl, downloadPath, isBreakPoint, listener);
		}		
		else
		{

	       if(isBreakPoint == true)
            {
                result = PluginTmpt.MITAC_BPDownloadFile(downloadUrl,downloadPath, listener);
            }
            else
    			result = PluginTmpt.MITAC_DownloadFile(downloadUrl, downloadPath, listener);
		}
		return result;
    };

	/** 
	* Cancel download process.
	* @return {Boolean} Cancel download process success or fail.
	* @example alert(new DeviceControl().downloadCancel()); 
	*/
    DeviceControl.prototype.downloadCancel = function() {
		var result = "";
		result = PluginTmpt.MITAC_ControlDownloadFile(0);
        if (result == errorList.get("E_MITAC_PLUGIN_OK")) {
            return true;
        }
		else {
            return false;
        }
    };
	
	DeviceControl.prototype.extractFile = function(isoPath,extractPath,extListener) { 
		isoPath = parseSlashes(isoPath);
		extractPath = parseSlashes(extractPath);

        var result = PluginTmpt.MITAC_ExtractPackage(isoPath,extractPath,extListener);
        if (result == errorList.get("E_MITAC_PLUGIN_OK")) {
            return true;
        }
		else {
            return false;
        }
    };
	/** 
	* Compare the plug-in version of client with the server side, returns TRUE or FALSE case.
	* @return {Boolean} The result of comparison.
	* @example alert(new DeviceControl().checkPlugin());
	* 1.  If the version of client plug-in is the same with the server OR the version of client plug-in is equal or newer than the server one.
	*	Returns TRUE.
	* 2. Other cases will always return FALSE.
	*	a. If the plug-in did not be installed in the client PC, returns FALSE.
	*	b. If the version of client plug-in is older than the server one, returns FALSE.
	*	c. If you forgot to put the version.js on your server, returns FALSE. (Important)
	*	d. If the version.js is corrupted on your server, returns FALSE.
	*	e. If there are some unknown reasons that cause the internet connection fails, returns FALSE.
	*/
	DeviceControl.prototype.checkPlugin = function() 
	{
		var xmlHttp;
		
		if (window.XMLHttpRequest)
			xmlHttp = new XMLHttpRequest();
		else if (window.ActiveXObject)
			xmlHttp = new ActiveXObject("Microsoft.XMLHTTP");

		xmlHttp.open("GET","version.js.php",false);
		xmlHttp.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
		xmlHttp.send(null);
		
		if(xmlHttp.readyState==4){
			if(xmlHttp.status==200){
				var serverRes = xmlHttp.responseText;
				var resObj = eval('(' + serverRes + ')');
				var winVersion = resObj.win;
				var macVersion = resObj.mac;
				var currentVersion;
				var tmpPlug = document.getElementById('PluginObject');
				var platform = window.navigator.platform;
				var sType = "application/mitac-device-sync";
				var o = this.getBrowserType();
				if (tmpPlug) {
					//Windows case
					if (platform === 'Win32') {
						//For IE
						if (o.ie) {
							if(tmpPlug.object != undefined) {
								var lock = tmpPlug.MITAC_UnlockPlugin('127.0.0.1','a34af967dfe6cce82755c48b310f554b');
								currentVersion = tmpPlug.MITAC_GetFileVersion("sss");
								var winVersionInt = parseInt(winVersion.replace(/\./g,""));
								var currentVersionInt = parseInt(currentVersion.replace(/\./g,""));
								
								if (currentVersionInt === winVersionInt || currentVersionInt > winVersionInt) {
									return true;
								}
								else {
									return false;
								}
							}
							else{
								return false;
							}
						}
						//For FF and Chrome on Windows
						else {
							if(navigator.mimeTypes) {
								if (navigator.mimeTypes[sType]) {
									if (navigator.mimeTypes[sType].enabledPlugin) {
										var lock = tmpPlug.MITAC_UnlockPlugin('127.0.0.1','a34af967dfe6cce82755c48b310f554b');
										currentVersion = tmpPlug.MITAC_GetFileVersion("sss");
										var winVersionInt = parseInt(winVersion.replace(/\./g,""));
										var currentVersionInt = parseInt(currentVersion.replace(/\./g,""));
										
										if (currentVersionInt === winVersionInt || currentVersionInt > winVersionInt) {
											return true;
										}
										else {
											return false;
										}
									}									
									else {
										return false;
									}
								}
								else {
									return false;
								}
							}
							else {
								return false;
							}
						}
					}
					else {
						//Mac case
						if(navigator.mimeTypes) {
							if (navigator.mimeTypes[sType]) {
								if (navigator.mimeTypes[sType].enabledPlugin) {
									var lock = tmpPlug.MITAC_UnlockPlugin('127.0.0.1','a34af967dfe6cce82755c48b310f554b');
									currentVersion = tmpPlug.MITAC_GetFileVersion("sss");
									var macVersionInt = parseInt(macVersion.replace(/\./g,""));
									var currentVersionInt = parseInt(currentVersion.replace(/\./g,""));
									
									if (currentVersionInt === macVersionInt || currentVersionInt > macVersionInt) {
										return true;
									}
									else {
										return false;
									}
								}
								else {
									return false;
								}
							}
							else {
								return false;
							}
						}
						else {
							return false;
						}
					}
				}
				else {
					return false;
				}
			}
			else{
				return false;
			}
		}
		else {
			return false;
		}
	}
}