using MySqlConnector;
using System.Data;
using System.Data.Common;

namespace EstiloLibre_CapaNegocio.AccesoBD
{
    public class ParametroBD: DbParameter
    {
        private readonly MySqlParameter _interno;
        public ParametroBD()
        {
            this._interno = new MySqlParameter();
        }

        public ParametroBD(string nombre, object? valor)
        {
            this._interno = new MySqlParameter(nombre, valor?? DBNull.Value);
        }

        public ParametroBD(string nombre, object? valor, MySqlDbType tipo)
        {
            this._interno = new MySqlParameter(nombre, valor ?? DBNull.Value);
            this._interno.MySqlDbType = tipo;
        }

        internal MySqlParameter ParametroInterno
        {
            get => this._interno;
        }

        public override DbType DbType { 
            get => this._interno.DbType; 
            set => this._interno.DbType = value; 
        }

        public override ParameterDirection Direction {
            get => this._interno.Direction;
            set => this._interno.Direction = value;
        }

        public override bool IsNullable {
            get => this._interno.IsNullable;
            set => this._interno.IsNullable = value;
        }

        public override string? ParameterName {
            get => this._interno.ParameterName;
            set => this._interno.ParameterName = value;
        }

        public override string? SourceColumn {
            get => this._interno.SourceColumn;
            set => this._interno.SourceColumn = value;
        }

        public override object? Value {
            get => this._interno.Value;
            set => this._interno.Value = value ?? DBNull.Value;
        }

        public override bool SourceColumnNullMapping {
            get => this._interno.SourceColumnNullMapping;
            set => this._interno.SourceColumnNullMapping = value;
        }

        public override int Size {
            get => this._interno.Size;
            set => this._interno.Size = value;
        }

        public override void ResetDbType()
        {
            this._interno.ResetDbType();
        }
    }
}
