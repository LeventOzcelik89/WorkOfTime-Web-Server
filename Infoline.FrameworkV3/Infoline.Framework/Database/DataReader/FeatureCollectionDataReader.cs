using NetTopologySuite.Features;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Infoline.Framework.Database.DataReader
{
    class FeatureCollectionDataReader : IDataReader
    {
        FeatureCollection _collection;
        string _geometryColumnName;
        IFeature _currentFeature;
        int _position;
        string[] _columns;
        Dictionary<string, int> _columnIndexes;
        int rowCount;

        public FeatureCollectionDataReader(FeatureCollection collection, string geoColName)
        {
            _position = -1;
            _collection = collection;
            _geometryColumnName = geoColName;
            rowCount = collection.Features.Count;

            if (collection.Features.Count > 0)
            {
                var firstFeature = collection.Features[0];
                _columns = firstFeature.Attributes.GetNames().Union(new string[] { _geometryColumnName }).ToArray();
                _columnIndexes = _columns.Select((a, i) => new { Key = a, Value = i }).ToDictionary(a => a.Key, a => a.Value);
            }
        }


        public object this[string name]
        {
            get
            {
                object result;
                if (name == _geometryColumnName)
                    result = Microsoft.SqlServer.Types.SqlGeography.STGeomFromWKB(new System.Data.SqlTypes.SqlBytes(_currentFeature.Geometry.AsBinary()), 4326);
                else
                    result = _currentFeature.Attributes[name];

                if (result == null)
                    return DBNull.Value;
                return result;
            }
        }

        public object this[int i]
        {
            get
            {
                return this[_columns[i]];
            }
        }

        public int Depth
        {
            get
            {
                return 1;
            }
        }

        public int FieldCount
        {
            get
            {
                return _columns.Length;
            }
        }

        public bool IsClosed
        {
            get
            {
                return false;
            }
        }

        public int RecordsAffected
        {
            get
            {
                return 0;
            }
        }

        public void Close()
        {
            
        }

        public void Dispose()
        {
            
        }

        public bool GetBoolean(int i)
        {
            return Convert.ToBoolean(this[i]);
        }

        public byte GetByte(int i)
        {
            return Convert.ToByte(this[i]);
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public char GetChar(int i)
        {
            return Convert.ToChar(this[i]);
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public IDataReader GetData(int i)
        {
            return this;
        }

        public string GetDataTypeName(int i)
        {
            return this[i].GetType().Name;
        }

        public DateTime GetDateTime(int i)
        {
            return (DateTime)this[i];
        }

        public decimal GetDecimal(int i)
        {
            return (decimal)this[i];
        }

        public double GetDouble(int i)
        {
            return (double)this[i];
        }

        public Type GetFieldType(int i)
        {
            return this[i].GetType();
        }

        public float GetFloat(int i)
        {
            return (float)this[i];
        }

        public Guid GetGuid(int i)
        {
            return (Guid)this[i];
        }

        public short GetInt16(int i)
        {
            return (short)this[i];
        }

        public int GetInt32(int i)
        {
            return (int)this[i];
        }

        public long GetInt64(int i)
        {
            return (long)this[i];
        }

        public string GetName(int i)
        {
            return _columns[i];
        }

        public int GetOrdinal(string name)
        {
            return _columnIndexes[name];
        }

        public DataTable GetSchemaTable()
        {
            throw new NotImplementedException();
        }

        public string GetString(int i)
        {
            return (string)this[i];
        }

        public object GetValue(int i)
        {
            return this[i];
        }

        public int GetValues(object[] values)
        {
            throw new NotImplementedException();
        }

        public bool IsDBNull(int i)
        {
            if (this[i] == DBNull.Value)
                return true;
            else
                return false;
        }

        public bool NextResult()
        {
            if (_position < rowCount - 1)
                return true;
            return false;
        }

        public bool Read()
        {
            if (NextResult())
            {
                _position += 1;
                _currentFeature = _collection.Features[_position];
                return true;
            }
            return false;
        }
    }
}
