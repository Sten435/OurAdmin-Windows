using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domein.DataBase.Table
{
	public class ColumnStructure
	{
        [JsonProperty("TABLE_CATALOG")]
        public string TABLECATALOG { get; set; }

        [JsonProperty("TABLE_SCHEMA")]
        public string TABLESCHEMA { get; set; }

        [JsonProperty("TABLE_NAME")]
        public string TABLENAME { get; set; }

        [JsonProperty("COLUMN_NAME")]
        public string COLUMNNAME { get; set; }

        [JsonProperty("ORDINAL_POSITION")]
        public int ORDINALPOSITION { get; set; }

        [JsonProperty("COLUMN_DEFAULT")]
        public object COLUMNDEFAULT { get; set; }

        [JsonProperty("IS_NULLABLE")]
        public string ISNULLABLE { get; set; }

        [JsonProperty("DATA_TYPE")]
        public string DATATYPE { get; set; }

        [JsonProperty("CHARACTER_MAXIMUM_LENGTH")]
        public long? CHARACTERMAXIMUMLENGTH { get; set; }

        [JsonProperty("CHARACTER_OCTET_LENGTH")]
        public long? CHARACTEROCTETLENGTH { get; set; }

        [JsonProperty("NUMERIC_PRECISION")]
        public int? NUMERICPRECISION { get; set; }

        [JsonProperty("NUMERIC_SCALE")]
        public int? NUMERICSCALE { get; set; }

        [JsonProperty("DATETIME_PRECISION")]
        public int? DATETIMEPRECISION { get; set; }

        [JsonProperty("CHARACTER_SET_NAME")]
        public string CHARACTERSETNAME { get; set; }

        [JsonProperty("COLLATION_NAME")]
        public string COLLATIONNAME { get; set; }

        [JsonProperty("COLUMN_TYPE")]
        public string COLUMNTYPE { get; set; }

        [JsonProperty("COLUMN_KEY")]
        public string COLUMNKEY { get; set; }

        [JsonProperty("EXTRA")]
        public string EXTRA { get; set; }

        [JsonProperty("PRIVILEGES")]
        public string PRIVILEGES { get; set; }

        [JsonProperty("COLUMN_COMMENT")]
        public string COLUMNCOMMENT { get; set; }

        [JsonProperty("IS_GENERATED")]
        public string ISGENERATED { get; set; }

        [JsonProperty("GENERATION_EXPRESSION")]
        public object GENERATIONEXPRESSION { get; set; }

    }
}
