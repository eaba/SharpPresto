using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using PrestoSharp.v1;

namespace PrestoSharp
{
    public class PrestoSqlDbDataReader : DbDataReader
    {
        internal PrestoSqlDbCommand DbCommand { get; set; }

        private int _mColumnCount = 0;
        private List<Column> _mColumns;
        private Dictionary<string, int> _mColumnIndex = new Dictionary<string, int>();

        private int _mRowCount = 0;
        private List<List<object>>  _mRows = null;
        private int _mRowIndex = -1;
        private object[] _mFields = null;

        internal PrestoSqlDbDataReader(PrestoSqlDbCommand Command)
        {
            DbCommand = Command;
        }
        

        public override bool Read()
        {
            if (_mRowIndex >= 0 && _mRowIndex < _mRows.Count() - 1)
            {
                _mRowIndex++;

                _mFields = _mRows[_mRowIndex].ToArray();

                return true;
            }

            var result = DbCommand.GetNextResult();
            while (result != null && result.Data == null)
                result = DbCommand.GetNextResult();

            if (_mColumns == null)
            {
                if (result != null)
                    _mColumns = result.Columns;
                if (_mColumns != null)
                {
                    _mColumnCount = _mColumns.Count;
                    var i = 0;
                    foreach (var col in _mColumns)
                        _mColumnIndex.Add(col.Name, i++);
                }
            }

            if (result != null)
            {
                _mRows = result.Data;
                _mRowCount += _mRows?.Count ?? 0;
                _mRowIndex = 0;
                _mFields = _mRows[_mRowIndex].ToArray();

                return true;
            }

            return false;
        }

        public override bool NextResult()
        {
            return false;
        }

        public override object this[int ordinal] => _mFields[ordinal];

        public override object this[string name] => _mColumnIndex.ContainsKey(name) ? _mFields[_mColumnIndex[name]] : null;

        public override int Depth => 0;
        public override int FieldCount => _mColumnCount;
        public override bool HasRows => _mRowCount > 0;
        public override bool IsClosed => false;
        public override int RecordsAffected => _mRowCount;

        public override string GetName(int ordinal)
        {
            if (ordinal >= 0 && ordinal < _mColumnCount)
                return _mColumns[ordinal].Name;
            return null;
        }
        public override Type GetFieldType(int ordinal)
        {
            if (ordinal >= 0 && ordinal < _mColumnCount)
                return StandardTypes.MapType(_mColumns[ordinal].TypeSignature.RawType);
            return null;
        }

        public override int GetOrdinal(string name)
        {
            if (_mColumnIndex.ContainsKey(name))
                return _mColumnIndex[name];
            return -1;
        }

        public override bool GetBoolean(int ordinal)
        {
            return Convert.ToBoolean(_mFields[ordinal]);
        }

        public override byte GetByte(int ordinal)
        {
            return Convert.ToByte(_mFields[ordinal]);
        }

        public override long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
        {
            throw new NotImplementedException();
        }

        public override char GetChar(int ordinal)
        {
            return Convert.ToChar(_mFields[ordinal]);
        }

        public override long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
        {
            throw new NotImplementedException();
        }

        public override string GetDataTypeName(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override DateTime GetDateTime(int ordinal)
        {
            return Convert.ToDateTime(_mFields[ordinal]);
        }

        public override decimal GetDecimal(int ordinal)
        {
            return Convert.ToDecimal(_mFields[ordinal]);
        }

        public override double GetDouble(int ordinal)
        {
            return Convert.ToDouble(_mFields[ordinal]);
        }

        public override IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public override float GetFloat(int ordinal)
        {
            return Convert.ToSingle(_mFields[ordinal]);
        }

        public override Guid GetGuid(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override short GetInt16(int ordinal)
        {
            return Convert.ToInt16(_mFields[ordinal]);
        }

        public override int GetInt32(int ordinal)
        {
            return Convert.ToInt32(_mFields[ordinal]);
        }

        public override long GetInt64(int ordinal)
        {
            return Convert.ToInt64(_mFields[ordinal]);
        }

        public override string GetString(int ordinal)
        {
            return Convert.ToString(_mFields[ordinal]);
        }

        public override object GetValue(int ordinal)
        {
            return StandardTypes.Convert(_mColumns[ordinal].TypeSignature.RawType, _mFields[ordinal]);
        }

        public override int GetValues(object[] values)
        {
            values = _mFields;

            return _mFields.Length;
        }

        public override bool IsDBNull(int ordinal)
        {
            return false;
        }

    }
}
