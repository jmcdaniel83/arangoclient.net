﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace ArangoDB.Client.Common.Newtonsoft.Json.Serialization
{
    internal class TraceJsonReader : JsonReader, IJsonLineInfo
    {
        private readonly JsonReader _innerReader;
        private readonly JsonTextWriter _textWriter;
        private readonly StringWriter _sw;

        public TraceJsonReader(JsonReader innerReader)
        {
            _innerReader = innerReader;

            _sw = new StringWriter(CultureInfo.InvariantCulture);
            _textWriter = new JsonTextWriter(_sw);
            _textWriter.Formatting = Formatting.Indented;
        }

        public string GetJson()
        {
            return _sw.ToString();
        }

        public override bool Read()
        {
            var value = _innerReader.Read();
            _textWriter.WriteToken(_innerReader, false, false);
            return value;
        }

        public override int? ReadAsInt32()
        {
            var value = _innerReader.ReadAsInt32();
            _textWriter.WriteToken(_innerReader, false, false);
            return value;
        }

        public override string ReadAsString()
        {
            var value = _innerReader.ReadAsString();
            _textWriter.WriteToken(_innerReader, false, false);
            return value;
        }

        public override byte[] ReadAsBytes()
        {
            var value = _innerReader.ReadAsBytes();
            _textWriter.WriteToken(_innerReader, false, false);
            return value;
        }

        public override decimal? ReadAsDecimal()
        {
            var value = _innerReader.ReadAsDecimal();
            _textWriter.WriteToken(_innerReader, false, false);
            return value;
        }

        public override DateTime? ReadAsDateTime()
        {
            var value = _innerReader.ReadAsDateTime();
            _textWriter.WriteToken(_innerReader, false, false);
            return value;
        }

#if !NET20
        public override DateTimeOffset? ReadAsDateTimeOffset()
        {
            var value = _innerReader.ReadAsDateTimeOffset();
            _textWriter.WriteToken(_innerReader, false, false);
            return value;
        }
#endif

        public override int Depth
        {
            get { return _innerReader.Depth; }
        }

        public override string Path
        {
            get { return _innerReader.Path; }
        }

        public override char QuoteChar
        {
            get { return _innerReader.QuoteChar; }
            protected internal set { _innerReader.QuoteChar = value; }
        }

        public override JsonToken TokenType
        {
            get { return _innerReader.TokenType; }
        }

        public override object Value
        {
            get { return _innerReader.Value; }
        }

        public override Type ValueType
        {
            get { return _innerReader.ValueType; }
        }

        public override void Close()
        {
            _innerReader.Close();
        }

        bool IJsonLineInfo.HasLineInfo()
        {
            IJsonLineInfo lineInfo = _innerReader as IJsonLineInfo;
            return lineInfo != null && lineInfo.HasLineInfo();
        }

        int IJsonLineInfo.LineNumber
        {
            get
            {
                IJsonLineInfo lineInfo = _innerReader as IJsonLineInfo;
                return (lineInfo != null) ? lineInfo.LineNumber : 0;
            }
        }

        int IJsonLineInfo.LinePosition
        {
            get
            {
                IJsonLineInfo lineInfo = _innerReader as IJsonLineInfo;
                return (lineInfo != null) ? lineInfo.LinePosition : 0;
            }
        }
    }
}