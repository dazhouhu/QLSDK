﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLSDK.Core
{
    public class QLMediaStatistics
    {
        public string ChannelName { get; set; }
        public string ParticipantName { get; set; }
        public string RemoteSystemId { get; set; }
        public string CallRate { get; set; }
        public string PacketsLost { get; set; }
        public string PacketLoss { get; set; }
        public string VideoProtocol { get; set; }
        public string VideoRate { get; set; }
        public string VideoRateUsed { get; set; }
        public string VideoFrameRate { get; set; }
        public string VideoPacketsLost { get; set; }
        public string VideoJitter { get; set; }
        public string VideoFormat { get; set; }
        public string ErrorConcealment { get; set; }
        public string AudioProtocol { get; set; }
        public string AudioRate { get; set; }
        public string AudioPacketsLost { get; set; }
        public string AudioJitter { get; set; }
        public string AudioEncrypt { get; set; }
        public string VideoEncrypt { get; set; }
        public string FeccEncrypt { get; set; }
        public string AudioReceivedPacket { get; set; }
        public string RoundTripTime { get; set; }
        public string FullIntraFrameRequest { get; set; }
        public string IntraFrameSent { get; set; }
        public string PacketsCount { get; set; }
        public string OverallCPULoad { get; set; }
        public int ChannelNum { get; set; }
    }
}
