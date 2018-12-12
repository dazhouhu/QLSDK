namespace QLSDK.Core
{
    public enum ErrorNumber
    {
        UNKNOWN = -1                                    //(-1),
        ,OK   							//0			/**< Indicates no error occurred. */
        ,ERR_INVALID_HANDLE     		//1			/**< Indicates the core handle or the call handle is invalid. */
        ,ERR_MOREDATA   				//2			/**< Indicates array size input is not large enough, and occurred in PLCM_MFW_EnumerateDevices, PLCM_MFW_GetSupportedCapabilities or PLCM_MFW_GetMediaStatistics.  */
        ,ERR_INVALID_DEVICETYPE   		//3			/**< Indicates device type is invalid. */
        ,ERR_NOTIMPLEMENTED     		//4			/**< Indicates function is not implemented. */
        ,ERR_NOTSUPPORTED       		//5			/**< Indicates feature is not supported. */
        ,ERR_INVALID_MESSAGEQ 			//6			/**< Indicates pointer to PLCM_MFW_EventQ is invalid. */
        ,ERR_INVALID_STREAMINFO 		//7			/**< Indicates pointer to PLCM_MFW_StreamInfo is invalid. */
        ,ERR_INVALID_WND   				//8			/**< Indicates handle represents the window is invalid. */
        ,ERR_INTERNAL   				//9			/**< Indicates internal error occurred in MFW Core. */
        ,ERR_INVALIDCALL 				//10          /**< Indicates call handle representing an invalid call. */
        ,ERR_INVALID_PARAMETER 			//11          /**< Indicates input parameter in function invoking is invalid. */
        ,ERR_WARNING_SENDING_CONTENT 	//12			/**< Indicates local site is sending content. */
        ,ERR_INVALID_DEVICE          	//13			/**< Indicates device is invalid. */
        ,ERR_INVALID_CAPS 				//14          /**< Indicates codec capabilities are invalid. This error occurs in getting or setting codec capabilities. */
        ,ERR_INVALID_KVLIST  			//15          /**< Indicates KVList is invalid. */
        ,ERR_UNSUPPORT_NEON  			//16          /**< Indicates Android device cannot support neon. This error occurs only for Android platforms. */
        ,ERR_GENERIC  				    //17          /**< Indicates a generic error. */
        ,ERR_ENCRYPTION_CONFIG          //18	        /**< This error occurs if PLCM_MFW_PlaceCall fails, indicating encryption configuration is not correct. */
        ,ERR_CALL_EXCEED_MAXIMUM_CALLS  //19	        /**< This error occurs if PLCM_MFW_PlaceCall fails, indicating the total call number exceeds the maximum calls. */
        ,ERR_CALL_IN_REGISTERING        //20          /**< This error occurs if PLCM_MFW_PlaceCall fails, indicating MFW Core is the registering server. */
        ,ERR_CALL_INVALID_FORMAT        //21	        /**< This error occurs if PLCM_MFW_PlaceCall fails, indicating the calling parameter is not correct. */
        ,ERR_CALL_NO_CONNECT            //22          /**< This error occurs if PLCM_MFW_PlaceCall fails, indicating the call can't connect to the server or far end. */
        ,ERR_CALL_HOST_UNKNOWN          //23	        /**< This error occurs if PLCM_MFW_PlaceCall fails, indicating failed to parse the host. */
        ,ERR_CALL_EXIST					//24			/**< This error occurs if PLCM_MFW_PlaceCall fails, indicates the call with the far end already exists. */
        ,ERR_INVALID_OPERATION          //25	        /**< Indicates operation is not applicable. */
        ,ERR_INVOKEAPI_INCALLBACK       //26          /**< Indicates the API function can't invoke inside a callback. */
        ,ERR_FILE_NON_EXIST	            //27			/**< Indicates the file does not exist.  */
        ,ERR_INVALID_RECPLY_ACTION      //28          /**< Indicates an invalid record/playback action.*/
        ,ERR_INVALID_PIPEID             //29          /**< Indicates an invalid Pipe ID for record. */
        ,ERR_UNSUPPORTED_FORMAT	        //30		    /**< Indicates the format of the file or data is not supported.  */
        ,ERR_KVLIST_INVALID_NAME        //31          /**< Indicates invalid server name or user name for KVList setting.  */                 
        ,ERR_KVLIST_INVALID_PORT        //32          /**< Indicates invalid port value for KVList setting.  */ 
        ,ERR_KVLIST_INVALID_PORTRANGE   //33          /**< Indicates invalid port range for KVList setting.  */
        ,ERR_KVLIST_INVALID_TLSOFFLOAD  //34          /**< Indicates invalid TLS OffLoad Setting for KVList setting.  */

        /*sample code error number, start from 129*/
        //INVALID_CALLHANDLE = -1,                              //(0xffffffff),
        ,ERRNO_START = 128,                                    //(128),
        LOADDLL_FAIL,                                    //(ERRNO_START.getValue() + 1),
        LOADDLL_FUNC_FAIL,                                    //(ERRNO_START.getValue() + 2),
        KVLISTINST_NULL,                                    //(ERRNO_START.getValue() + 3),
        CALLBACK_NULL,                                    //(ERRNO_START.getValue() + 4),
        REGISTER_FAIL,                                    //(ERRNO_START.getValue() + 5),
        SHARE_CONTENT_FAIL,                                    //(ERRNO_START.getValue() + 6),
        UPDATECONFIG_FAIL,                                    //(ERRNO_START.getValue() + 7),
        NULL_CALLHANDLE,                                    //(ERRNO_START.getValue() + 8),
        NULL_DEVICE,                                    //(ERRNO_START.getValue() + 9),
        NULL_CALLEE_ADDR,                                    //(ERRNO_START.getValue() + 10),
        NULL_DEVICE_NAME                                    //(ERRNO_START.getValue() + 11);
                                                                        //MORE...add sample error code
    }
}
