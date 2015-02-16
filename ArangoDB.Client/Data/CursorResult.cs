﻿using ArangoDB.Client.Common.Newtonsoft.Json;
using ArangoDB.Client.Common.Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArangoDB.Client.Data
{
    public class CursorResult : BaseResult
    {
        public int RequestCount { get; set; }

        public bool HasMore { get; set; }

        public long? Count { get; set; }

        public string Id { get; set; }

        public CursorExtraResult Extra { get; set; }

        public CursorResult()
        {
            this.Extra = new CursorExtraResult();
        }

        internal void SetFromJsonTextReader(string name, JsonToken token, object value)
        {
            if (name == "extra")
                this.Extra = (value as JObject).ToObject<CursorExtraResult>();

            if (name == "code" && token == JsonToken.Integer)
                this.Code = Convert.ToInt32(value);

            if (name == "error" && token == JsonToken.Boolean)
                this.Error = Convert.ToBoolean(value);

            if (name == "errorMessage" && token == JsonToken.String)
                this.ErrorMessage = value.ToString();

            if (name == "errorNum" && token == JsonToken.Integer)
                this.ErrorNum = Convert.ToInt32(value);

            if (name == "hasMore" && token == JsonToken.Boolean)
                this.HasMore = Convert.ToBoolean(value);

            if (name == "count" && token == JsonToken.Integer)
                this.Count = Convert.ToInt32(value);

            if (name == "id" && token == JsonToken.String)
                this.Id = value.ToString();
        }
    }

    public class CursorExtraResult
    {
        public CursorExtraStatsResult Stats { get; set; }

        public CursorExtraResult()
        {
            this.Stats = new CursorExtraStatsResult();
        }
    }

    public class CursorExtraStatsResult
    {
        public long? FullCount { get; set; }

        public int? WritesExecuted { get; set; }

        public int? WritesIgnored { get; set; }

        public int? ScannedFull { get; set; }

        public int? ScannedIndex { get; set; }
    }
}
