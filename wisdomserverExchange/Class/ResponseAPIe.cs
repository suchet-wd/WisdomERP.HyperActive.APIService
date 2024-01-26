using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wisdomserverExchange.Class
{
    class ResponseAPIe
    {
        private string type;
        private string title;
        private string status;
        private string traceId;
        private List<string> errors;

        public ResponseAPIe(string type, string title, string status, string traceId, List<string> errors)
        {
            this.type = type;
            this.title = title;
            this.status = status;
            this.traceId = traceId;
            this.errors = errors;
        }

        public string Type { get => type; set => type = value; }
        public string Title { get => title; set => title = value; }
        public string Status { get => status; set => status = value; }
        public string TraceId { get => traceId; set => traceId = value; }
        public List<string> Errors { get => errors; set => errors = value; }
    }
}
