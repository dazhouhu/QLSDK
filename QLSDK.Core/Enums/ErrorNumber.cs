namespace QLSDK.Core
{
    internal enum ErrorNumber
    {
        /// <summary>
        /// 未知
        /// </summary>
        UNKNOWN = -1,
        /// <summary>
        /// 成功
        /// </summary>
        OK,                             //0         /**< Indicates no error occurred. */
        /// <summary>
        /// 无效句柄
        /// </summary>
        ERR_INVALID_HANDLE,             //1         /**< Indicates the core handle or the call handle is invalid. */
        /// <summary>
        /// 超出大小，主要发生在PLCM_MFW_EnumerateDevices, PLCM_MFW_GetSupportedCapabilities or PLCM_MFW_GetMediaStatistics中
        /// </summary>
        ERR_MOREDATA,                   //2         /**< Indicates array size input is not large enough, and occurred in PLCM_MFW_EnumerateDevices, PLCM_MFW_GetSupportedCapabilities or PLCM_MFW_GetMediaStatistics.  */
        /// <summary>
        /// 设备类型无效
        /// </summary>
        ERR_INVALID_DEVICETYPE,   		//3			/**< Indicates device type is invalid. */
        /// <summary>
        /// 未实现
        /// </summary>
        ERR_NOTIMPLEMENTED,     		//4			/**< Indicates function is not implemented. */
        /// <summary>
        /// 不支持
        /// </summary>
        ERR_NOTSUPPORTED,       		//5			/**< Indicates feature is not supported. */
        /// <summary>
        /// 事件无效
        /// </summary>
        ERR_INVALID_MESSAGEQ, 			//6			/**< Indicates pointer to PLCM_MFW_EventQ is invalid. */
        /// <summary>
        /// 流无效
        /// </summary>
        ERR_INVALID_STREAMINFO, 		//7			/**< Indicates pointer to PLCM_MFW_StreamInfo is invalid. */
        /// <summary>
        /// 窗口句柄无效
        /// </summary>
        ERR_INVALID_WND,   				//8			/**< Indicates handle represents the window is invalid. */
        /// <summary>
        /// 内容错误
        /// </summary>
        ERR_INTERNAL,   				//9			/**< Indicates internal error occurred in MFW Core. */
        /// <summary>
        /// 无效呼叫
        /// </summary>
        ERR_INVALIDCALL,                //10          /**< Indicates call handle representing an invalid call. */
        /// <summary>
        /// 参数无效
        /// </summary>
        ERR_INVALID_PARAMETER,          //11          /**< Indicates input parameter in function invoking is invalid. */
        /// <summary>
        /// 发送共享中
        /// </summary>
        ERR_WARNING_SENDING_CONTENT,    //12		    /**< Indicates local site is sending content. */
        /// <summary>
        /// 设备无效
        /// </summary>
        ERR_INVALID_DEVICE,             //13			/**< Indicates device is invalid. */
        /// <summary>
        /// 编码无效
        /// </summary>
        ERR_INVALID_CAPS,               //14          /**< Indicates codec capabilities are invalid. This error occurs in getting or setting codec capabilities. */
        /// <summary>
        /// 配置参数无效
        /// </summary>
        ERR_INVALID_KVLIST,             //15          /**< Indicates KVList is invalid. */
        /// <summary>
        /// 不支持NEON
        /// </summary>
        ERR_UNSUPPORT_NEON,             //16          /**< Indicates Android device cannot support neon. This error occurs only for Android platforms. */
        /// <summary>
        /// 一般错误
        /// </summary>
        ERR_GENERIC,                    //17          /**< Indicates a generic error. */
        /// <summary>
        /// 加密配置错误
        /// </summary>
        ERR_ENCRYPTION_CONFIG,          //18	        /**< This error occurs if PLCM_MFW_PlaceCall fails, indicating encryption configuration is not correct. */
        /// <summary>
        /// 呼叫数超出限定
        /// </summary>
        ERR_CALL_EXCEED_MAXIMUM_CALLS,  //19	        /**< This error occurs if PLCM_MFW_PlaceCall fails, indicating the total call number exceeds the maximum calls. */
        /// <summary>
        /// 注册中
        /// </summary>
        ERR_CALL_IN_REGISTERING,        //20          /**< This error occurs if PLCM_MFW_PlaceCall fails, indicating MFW Core is the registering server. */
        /// <summary>
        /// 格式错误
        /// </summary>
        ERR_CALL_INVALID_FORMAT,        //21	        /**< This error occurs if PLCM_MFW_PlaceCall fails, indicating the calling parameter is not correct. */
        /// <summary>
        /// 未连接
        /// </summary>
        ERR_CALL_NO_CONNECT,            //22          /**< This error occurs if PLCM_MFW_PlaceCall fails, indicating the call can't connect to the server or far end. */
        /// <summary>
        /// 服务器未知
        /// </summary>
        ERR_CALL_HOST_UNKNOWN,          //23	        /**< This error occurs if PLCM_MFW_PlaceCall fails, indicating failed to parse the host. */
        /// <summary>
        /// 呼叫已经存在
        /// </summary>
        ERR_CALL_EXIST,                 //24			/**< This error occurs if PLCM_MFW_PlaceCall fails, indicates the call with the far end already exists. */
        /// <summary>
        /// 无效操作
        /// </summary>
        ERR_INVALID_OPERATION,          //25	        /**< Indicates operation is not applicable. */
        /// <summary>
        /// 回调中不能调用API
        /// </summary>
        ERR_INVOKEAPI_INCALLBACK,       //26          /**< Indicates the API function can't invoke inside a callback. */
        /// <summary>
        /// 文件不存在
        /// </summary>
        ERR_FILE_NON_EXIST,             //27			/**< Indicates the file does not exist.  */
        /// <summary>
        /// 无效记录/回放操作
        /// </summary>
        ERR_INVALID_RECPLY_ACTION,      //28          /**< Indicates an invalid record/playback action.*/
        /// <summary>
        /// 无效通道
        /// </summary>
        ERR_INVALID_PIPEID,             //29          /**< Indicates an invalid Pipe ID for record. */
        /// <summary>
        /// 格式不支持
        /// </summary>
        ERR_UNSUPPORTED_FORMAT,         //30		    /**< Indicates the format of the file or data is not supported.  */
        /// <summary>
        /// 名称无效
        /// </summary>
        ERR_KVLIST_INVALID_NAME,        //31          /**< Indicates invalid server name or user name for KVList setting.  */                 
        /// <summary>
        /// 端口无效
        /// </summary>
        ERR_KVLIST_INVALID_PORT,        //32          /**< Indicates invalid port value for KVList setting.  */ 
        /// <summary>
        /// 端口范围无效
        /// </summary>
        ERR_KVLIST_INVALID_PORTRANGE,   //33          /**< Indicates invalid port range for KVList setting.  */
        /// <summary>
        /// TLS离线无效
        /// </summary>
        ERR_KVLIST_INVALID_TLSOFFLOAD  //34          /**< Indicates invalid TLS OffLoad Setting for KVList setting.  */
    }
}
