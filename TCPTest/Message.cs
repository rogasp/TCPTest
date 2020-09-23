using System;
using System.Collections.Generic;
using System.Text;
using TCPTest.Messages;

namespace TCPTest
{
    
    class Message
    {
        public enum MessageTypes
        {
            MSG_NEXT_MATCH = 1,
            MSG_RESULT,
            MSG_ACK,
            MSG_SET_COMMENT,
            MSG_SET_POINTS,  // 5
            MSG_HELLO,
            MSG_DUMMY,
            MSG_MATCH_INFO,
            MSG_NAME_INFO,
            MSG_NAME_REQ,    // 10
            MSG_ALL_REQ,
            MSG_CANCEL_REST_TIME,
            MSG_UPDATE_LABEL,
            MSG_EDIT_COMPETITOR,
            MSG_SCALE,       // 15
            MSG_11_MATCH_INFO,
            MSG_EVENT,
            MSG_WEB,
            MSG_LANG,
            MSG_LOOKUP_COMP, // 20
            MSG_INPUT,
            MSG_LABELS,
            NUM_MESSAGES
        };

        public enum Label
        {
             START_BIG           = 128,
             STOP_BIG            = 129,
             START_ADVERTISEMENT = 130,
             START_COMPETITORS   = 131,
             STOP_COMPETITORS    = 132,
             START_WINNER        = 133,
             STOP_WINNER         = 134,
             SAVED_LAST_NAMES    = 135,
             SHOW_MESSAGE        = 136,
             SET_SCORE           = 137,
             SET_POINTS          = 138,
             SET_OSAEKOMI_VALUE  = 139,
             SET_TIMER_VALUE     = 140,
             SET_TIMER_OSAEKOMI_COLOR = 141,
             SET_TIMER_RUN_COLOR = 142
        }

        public long SrcIpAddr { get; set; }
        public int Type { get; set; }
        public int Sender { get; set; }
        public UpdateLabel MsgUpdateLabel{ get; set; }

        public Message()
        {
            MsgUpdateLabel = new UpdateLabel();
        }
    }
}
