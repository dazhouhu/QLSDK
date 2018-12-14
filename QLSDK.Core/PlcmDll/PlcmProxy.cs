using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLSDK.Core
{
    internal class PlcmProxy
    {
        public static ErrorNumber FreeEvent(IntPtr evt)
        {
            return (ErrorNumber)PlcmHelper.freeEvent(evt);
        }
        public static ErrorNumber SetProperty(PropertyKey key, string value)
        {
            return (ErrorNumber)PlcmHelper.addProperty((int)key, value);
        }

        public static ErrorNumber InstallCallback(AddEventCallback addEvent, DispatchEventsCallback dispatchEvents, AddLogCallback addLog,
                                            AddDeviceCallback addDevice, DisplayMediaStatisticsCallback displayMediaStatistics, DisplayCallStatisticsCallback displayCallStatistics,
                                            DisplayCodecCapabilities displayCodecNamesCallback, AddAppCallback addAppCallback)
        {
            return (ErrorNumber)PlcmHelper.installCallback(addEvent, dispatchEvents, addLog, addDevice, displayMediaStatistics, displayCallStatistics, displayCodecNamesCallback, addAppCallback);
        }

        public static ErrorNumber PreInitialize()
        {
            return (ErrorNumber)PlcmHelper.preInitialize();
        }

        public static ErrorNumber Initialize()
        {
            return (ErrorNumber)PlcmHelper.initialize();
        }

        public static ErrorNumber SetAudioDevice(string micHandle, string speakerHandle)
        {
            return (ErrorNumber)PlcmHelper.setAudioDevice(micHandle, speakerHandle);
        }

        public static ErrorNumber SetAudioDeviceForRingtone(string speakerHandle)
        {
            return (ErrorNumber)PlcmHelper.setAudioDeviceForRingtone(speakerHandle);
        }

        public static ErrorNumber SetVideoDevice(string cameraHandle)
        {
            return (ErrorNumber)PlcmHelper.setVideoDevice(cameraHandle);
        }

        public static ErrorNumber PlaceCall(string callee, ref int callHandle, CallMode callMode)
        {
            return (ErrorNumber)PlcmHelper.placeCall(callee, ref callHandle, (int)callMode);
        }

        public static ErrorNumber TerminateCall(int callHandle)
        {
            return (ErrorNumber)PlcmHelper.terminateCall(callHandle);
        }

        public static ErrorNumber AnswerCall(int callHandle, CallMode callMode, string authToken, string cryptoSuiteType, string srtpKey, bool sutLiteEnable)
        {
            return (ErrorNumber)PlcmHelper.answerCall(callHandle, (int)callMode, authToken, cryptoSuiteType, srtpKey, sutLiteEnable);
        }
        public static ErrorNumber AnswerCall(QLCall call,CallMode callMode)
        {
            string cryptoSuiteType = "AES_CM_128_HMAC_SHA1_80";
            string srtpKey = "HfVGG79oW5XStt9DewUYrdngYlV/QqDBGIDNFB7m";
            var authToken = "AApzdG1lZXRpbmcxAAdzdHVzZXIxAAABPcJe1o4CsXgvirq1RQys3JCU0U8RvJ4uoA==";

            return AnswerCall(call.CallHandle, callMode, authToken, cryptoSuiteType, srtpKey, true);
        }

        public static ErrorNumber RejectCall(int callHandle)
        {
            return (ErrorNumber)PlcmHelper.rejectCall(callHandle);
        }

        public static ErrorNumber AttachStreamWnd(MediaType mediaType, int streamId, int callHandle, IntPtr windowHandle, int x, int y, int width, int height)
        {
            return (ErrorNumber)PlcmHelper.setStreamInfo((int)mediaType, streamId, callHandle, windowHandle, x, y, width, height);
        }

        public static ErrorNumber DetachStreamWnd(MediaType mediaType, int streamId, int callHandle)
        {
            return (ErrorNumber)PlcmHelper.detachStreamWnd((int)mediaType, streamId, callHandle);
        }

        public static ErrorNumber HoldCall(int callHandle)
        {
            return (ErrorNumber)PlcmHelper.holdCall(callHandle);
        }

        public static ErrorNumber ResumeCall(int callHandle)
        {
            return (ErrorNumber)PlcmHelper.resumeCall(callHandle);
        }

        public static ErrorNumber MuteMic(int callhandle, bool isMute)
        {
            return (ErrorNumber)PlcmHelper.muteMic(callhandle, isMute);
        }

        public static ErrorNumber MuteSpeaker(bool isMute)
        {
            return (ErrorNumber)PlcmHelper.muteSpeaker(isMute);
        }

        public static ErrorNumber SetMicVolume(int volume)
        {
            return (ErrorNumber)PlcmHelper.setMicVolume((uint)volume);
        }

        public static int GetMicVolume()
        {
            return (int)PlcmHelper.getMicVolume();
        }

        public static ErrorNumber SetSpeakerVolume(int volume)
        {
            return (ErrorNumber)PlcmHelper.setSpeakerVolume((uint)volume);
        }

        public static int GetSpeakerVolume()
        {
            return (int)PlcmHelper.getSpeakerVolume();
        }
        public static ErrorNumber RegisterClient()
        {
            return (ErrorNumber)PlcmHelper.registerClient();
        }
        public static void UnregisterClient()
        {
            PlcmHelper.unregisterClient();
        }

        public static ErrorNumber UpdateConfig()
        {
            return (ErrorNumber)PlcmHelper.updateConfig();
        }

        public static ErrorNumber StartShareContent(int callhandle, string deviceHandle, IntPtr appWndHandle)
        {
            return (ErrorNumber)PlcmHelper.startShareContent(callhandle, deviceHandle, appWndHandle);
        }

        public static ErrorNumber StartBFCPContent(int callhandle)
        {
            return (ErrorNumber)PlcmHelper.startBFCPContent(callhandle);
        }

        public static ErrorNumber StopShareContent(int callhandle)
        {
            return (ErrorNumber)PlcmHelper.stopShareContent(callhandle);
        }

        public static ErrorNumber SetContentBuffer(ImageFormat format, int width, int height)
        {
            return (ErrorNumber)PlcmHelper.setContentBuffer((int)format, width, height);
        }

        public static ErrorNumber DestroyExit()
        {
            return (ErrorNumber)PlcmHelper.destroyExit();
        }

        public static ErrorNumber GetMediaStatistics(int callhandle)
        {
            return (ErrorNumber)PlcmHelper.getMediaStatistics(callhandle);
        }

        public static ErrorNumber GetCallStatistics()
        {
            return (ErrorNumber)PlcmHelper.getCallStatistics();
        }

        public static string GetVersion()
        {
            var intPtrVersion = PlcmHelper.getVersion();
            return IntPtrHelper.IntPtrToUTF8string(intPtrVersion);
        }

        public static ErrorNumber SendFECCKey(int callhandle, FECCKey key, FECCAction action)
        {
            return (ErrorNumber)PlcmHelper.sendFECCKey(callhandle, (int)key, (int)action);
        }

        public static ErrorNumber SendDTMFKey(int callHandle, DTMFKey key)
        {
            return (ErrorNumber)PlcmHelper.sendDTMFKey(callHandle, (int)key);
        }

        public static ErrorNumber GetDevice(DeviceType deviceType)
        {
            return (ErrorNumber)PlcmHelper.getDeviceEnum((int)deviceType);
        }

        public static ErrorNumber GetSupportedCapabilities()
        {
            return (ErrorNumber)PlcmHelper.getSupportedCapabilities();
        }

        public static ErrorNumber SetCapabilitiesEnable(int size, string type, string name, string tagID)
        {
            return (ErrorNumber)PlcmHelper.setCapabilitiesEnable(size, type, name, tagID);
        }

        public static ErrorNumber SetPreferencesCapabilities(int size, string type, string name, string tagID)
        {
            return (ErrorNumber)PlcmHelper.setPreferencesCapabilities(size, type, name, tagID);
        }

        public static ErrorNumber MuteLocalVideo(bool isMute)
        {
            return (ErrorNumber)PlcmHelper.MuteLocalVideo(isMute);
        }

        public static ErrorNumber GetApplicationInfo()
        {
            return (ErrorNumber)PlcmHelper.getApplicationInfo();
        }

        public static ErrorNumber StartCamera()
        {
            return (ErrorNumber)PlcmHelper.startCamera();
        }

        public static ErrorNumber StopCamera()
        {
            return (ErrorNumber)PlcmHelper.stopCamera();
        }

        public static ErrorNumber StartRecord(int callHandle, RecordPipeType pipeType, string filePath)
        {
            return (ErrorNumber)PlcmHelper.startRecord(callHandle, (int)pipeType, filePath);
        }

        public static ErrorNumber StopRecord(int callHandle)
        {
            return (ErrorNumber)PlcmHelper.stopRecord(callHandle);
        }

        public static ErrorNumber PauseRecord(int callHandle)
        {
            return (ErrorNumber)PlcmHelper.pauseRecord(callHandle);
        }

        public static ErrorNumber ResumeRecord(int callHandle)
        {
            return (ErrorNumber)PlcmHelper.resumeRecord(callHandle);
        }

        public static ErrorNumber StartPlayback(string filePath)
        {
            return (ErrorNumber)PlcmHelper.startPlayback(filePath);
        }

        public static ErrorNumber StopPlayback()
        {
            return (ErrorNumber)PlcmHelper.stopPlayback();
        }

        public static ErrorNumber PausePlayback()
        {
            return (ErrorNumber)PlcmHelper.pausePlayback();
        }

        public static ErrorNumber ResumePlayback()
        {
            return (ErrorNumber)PlcmHelper.resumePlayback();
        }

        public static ErrorNumber SetPlayPosition(int percent)
        {
            return (ErrorNumber)PlcmHelper.setPlayPosition(percent);
        }

        public static ErrorNumber GetPlayPosition(ref int percent)
        {
            return (ErrorNumber)PlcmHelper.getPlayPosition(ref percent);
        }

        public static ErrorNumber GetFileDuration(ref int seconds)
        {
            return (ErrorNumber)PlcmHelper.getFileDuration(ref seconds);
        }


        public static ErrorNumber GetRecordStatus(int callhandle, ref int status, ref int pipeType, string fileName)
        {
            return (ErrorNumber)PlcmHelper.getRecordStatus(callhandle, ref status, ref pipeType, fileName);
        }

        public static ErrorNumber SetRemoteOneSVCVideoStream(int callhandle, int selectMode, int streamId, bool isActiveSpeaker)
        {
            return (ErrorNumber)PlcmHelper.setRemoteOneSVCVideoStream(callhandle, selectMode, streamId, isActiveSpeaker);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callhandle"></param>
        /// <param name="selectMode">0:自动,1:手动</param>
        /// <param name="streamNumber"></param>
        /// <returns></returns>
        public static ErrorNumber SetRemoteVideoStreamNumber(int callhandle, int selectMode, int streamNumber)
        {
            return (ErrorNumber)PlcmHelper.setRemoteVideoStreamNumber(callhandle, selectMode, streamNumber);
        }


        public static ErrorNumber StartPlayAlert(string filePath, bool isLoop, int interval)
        {
            return (ErrorNumber)PlcmHelper.startPlayAlert(filePath, isLoop, interval);
        }

        public static ErrorNumber StopPlayAlert()
        {
            return (ErrorNumber)PlcmHelper.stopPlayAlert();
        }

        public static ErrorNumber ChangeCallMode(int callHandle, CallMode callmode)
        {
            return (ErrorNumber)PlcmHelper.changeCallMode(callHandle, (int)callmode);
        }

        public static ErrorNumber PopupCameraPropertyAdvancedSettings(IntPtr winHandle)
        {
            return (ErrorNumber)PlcmHelper.popupCameraPropertyAdvancedSettings(winHandle);
        }

        public static ErrorNumber SetCertificateChoice(string certFingerPrint, int confirmId, CertificateChoiceType userChoice)
        {
            return (ErrorNumber)PlcmHelper.setCertificateChoice(certFingerPrint, confirmId, (int)userChoice);
        }

        public static ErrorNumber SetConfigFilePath(string filePath)
        {
            return (ErrorNumber)PlcmHelper.setConfigFilePath(filePath);
        }

        public static IntPtr StartTranscoder(int audioOnly, LayoutType layoutType, int resoFormat, int bitRate,
                 int frameRate, int keyFrameInterval, string inputFileName, string outputFileName, ref int errNo)
        {
            return PlcmHelper.startTranscoder(audioOnly, (int)layoutType, resoFormat, bitRate, frameRate, keyFrameInterval, inputFileName, outputFileName, ref errNo);
        }

        public static ErrorNumber StopTranscoder(IntPtr taskHandle)
        {
            return (ErrorNumber)PlcmHelper.stopTranscoder(taskHandle);
        }

        public static ErrorNumber GetProgressOfTranscoder(IntPtr taskHandle, ref int percentage)
        {
            return (ErrorNumber)PlcmHelper.getProgressOfTranscoder(taskHandle, ref percentage);
        }

        public static ErrorNumber SetCallStream(CallStreamType type, string filePath)
        {
            return (ErrorNumber)PlcmHelper.setCallStream((int)type, filePath);
        }

        public static ErrorNumber ClearCallStream(CallStreamType type)
        {
            return (ErrorNumber)PlcmHelper.clearCallStream((int)type);
        }

        public static ErrorNumber EnableRecordAudioStreamCallback(int callHandle, RecordAudioStreamCallback callBack, int format, int interval)
        {
            return (ErrorNumber)PlcmHelper.enableRecordAudioStreamCallback(callHandle, callBack, format, interval);
        }


        public static ErrorNumber DisableRecordAudioStreamCallback(int callHandle)
        {
            return (ErrorNumber)PlcmHelper.disableRecordAudioStreamCallback(callHandle);
        }

        public static ErrorNumber SetStaticImage(IntPtr buffer, int length, int width, int height)
        {
            return (ErrorNumber)PlcmHelper.setStaticImage(buffer, length, width, height);
        }

        public static ErrorNumber EnableMediaQoE(VideoDataCapturedCallback videoDataCaptured,
                                            VideoDataRenderedCallback videoDataRendered,
                                            SpeakerDataReceivedCallback speakerDataReceived,
                                            MicrophoneDataSentCallback microphoneDataSent,
                                            DataEncodedCallback dataEncoded,
                                            DataDecodedCallback dataDecoded,
                                            RtpPacketReceivedCallback rtpPacketReceived,
                                            RtpPacketSentCallback rtpPacketSent,
                                            QoEType type)
        {
            return (ErrorNumber)PlcmHelper.enableMediaQoE(videoDataCaptured, videoDataRendered, speakerDataReceived, microphoneDataSent, dataEncoded, dataDecoded, rtpPacketReceived, rtpPacketSent, (int)type);
        }

        public static ErrorNumber DisableMediaQoE(QoEType type)
        {
            return (ErrorNumber)PlcmHelper.disableMediaQoE((int)type);
        }
        public static ErrorNumber startHttpTunnelAutoDiscovery(string destAddress, string destPort, string regId, string destUser)
        {
            return (ErrorNumber)PlcmHelper.startHttpTunnelAutoDiscovery(destAddress, destPort, regId, destUser);

        }
    }
}
