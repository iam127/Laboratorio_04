using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;

namespace NeptunoWPF
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public int IdProveedor { get; set; }
        public int IdCategoria { get; set; }
        public string CantidadPorUnidad { get; set; }
        public decimal PrecioUnidad { get; set; }
        public short UnidadesEnExistencia { get; set; }
        public short UnidadesEnPedido { get; set; }
        public short NivelNuevoPedido { get; set; }
        public bool Suspendido { get; set; }
        public string CategoriaProducto { get; set; }
    }

    public class Categoria
    {
        public int IdCategoria { get; set; }
        public string NombreCategoria { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
        public string CodCategoria { get; set; }
    }

    public class Proveedor
    {
        public int IdProveedor { get; set; }
        public string NombreCompania { get; set; }
        public string NombreContacto { get; set; }
        public string CargoContacto { get; set; }
        public string Direccion { get; set; }
        public string Ciudad { get; set; }
        public string Region { get; set; }
        public string CodPostal { get; set; }
        public string Pais { get; set; }
        public string Telefono { get; set; }
        public string Fax { get; set; }
    }

    public class DetallePedido
    {
        public int IdPedido { get; set; }
        public int IdProducto { get; set; }
        public decimal PrecioUnidad { get; set; }
        public short Cantidad { get; set; }
        public float Descuento { get; set; }
        public System.DateTime FechaPedido { get; set; }
    }

    public class NeptunoDataAccess
    {
        private readonly string _connectionString;

        public NeptunoDataAccess(string connectionString)
        {
            _connectionString = connectionString;
        }

        public ObservableCollection<Producto> ListarProductos()
        {
            var lista = new ObservableCollection<Producto>();

            using (var conexion = new SqlConnection(_connectionString))
            using (var comando = new SqlCommand("sp_ListarProductos", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;
                conexion.Open();

                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        lista.Add(new Producto
                        {
                            IdProducto = lector.GetInt32(lector.GetOrdinal("idProducto")),
                            NombreProducto = lector.GetString(lector.GetOrdinal("NombreProducto")),
                            IdProveedor = lector.IsDBNull(lector.GetOrdinal("idProveedor")) ? 0 : lector.GetInt32(lector.GetOrdinal("idProveedor")),
                            IdCategoria = lector.IsDBNull(lector.GetOrdinal("idCategoria")) ? 0 : lector.GetInt32(lector.GetOrdinal("idCategoria")),
                            CantidadPorUnidad = lector.IsDBNull(lector.GetOrdinal("CantidadPorUnidad")) ? "" : lector.GetString(lector.GetOrdinal("CantidadPorUnidad")),
                            PrecioUnidad = lector.IsDBNull(lector.GetOrdinal("PrecioUnidad")) ? 0 : lector.GetDecimal(lector.GetOrdinal("PrecioUnidad")),
                            UnidadesEnExistencia = lector.IsDBNull(lector.GetOrdinal("UnidadesEnExistencia")) ? (short)0 : lector.GetInt16(lector.GetOrdinal("UnidadesEnExistencia")),
                            UnidadesEnPedido = lector.IsDBNull(lector.GetOrdinal("UnidadesEnPedido")) ? (short)0 : lector.GetInt16(lector.GetOrdinal("UnidadesEnPedido")),
                            NivelNuevoPedido = lector.IsDBNull(lector.GetOrdinal("nivelNuevoPedido")) ? (short)0 : lector.GetInt16(lector.GetOrdinal("nivelNuevoPedido")),
                            Suspendido = !lector.IsDBNull(lector.GetOrdinal("suspendido")) && lector.GetInt16(lector.GetOrdinal("suspendido")) != 0,
                            CategoriaProducto = lector.IsDBNull(lector.GetOrdinal("categoriaProducto")) ? "" : lector.GetString(lector.GetOrdinal("categoriaProducto"))
                        });
                    }
                }
            }

            return lista;
        }

        public ObservableCollection<Categoria> ListarCategorias()
        {
            var lista = new ObservableCollection<Categoria>();

            using (var conexion = new SqlConnection(_connectionString))
            using (var comando = new SqlCommand("sp_ListarCategorias", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;
                conexion.Open();

                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        lista.Add(new Categoria
                        {
                            IdCategoria = lector.GetInt32(lector.GetOrdinal("idcategoria")),
                            NombreCategoria = lector.GetString(lector.GetOrdinal("nombrecategoria")),
                            Descripcion = lector.IsDBNull(lector.GetOrdinal("descripcion")) ? "" : lector.GetString(lector.GetOrdinal("descripcion")),
                            Activo = !lector.IsDBNull(lector.GetOrdinal("Activo")) && lector.GetBoolean(lector.GetOrdinal("Activo")),
                            CodCategoria = lector.IsDBNull(lector.GetOrdinal("CodCategoria")) ? "" : lector.GetString(lector.GetOrdinal("CodCategoria"))
                        });
                    }
                }
            }

            return lista;
        }

        public ObservableCollection<Proveedor> ListarProveedores()
        {
            var lista = new ObservableCollection<Proveedor>();

            using (var conexion = new SqlConnection(_connectionString))
            using (var comando = new SqlCommand("sp_ListarProveedores", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;
                conexion.Open();

                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        lista.Add(LeerProveedor(lector));
                    }
                }
            }

            return lista;
        }

        public ObservableCollection<Proveedor> BuscarProveedoresPorContactoYCiudad(string nombreContacto, string ciudad)
        {
            var lista = new ObservableCollection<Proveedor>();

            using (var conexion = new SqlConnection(_connectionString))
            using (var comando = new SqlCommand("sp_BuscarProveedoresPorContactoYCiudad", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@NombreContacto", nombreContacto);
                comando.Parameters.AddWithValue("@Ciudad", ciudad);
                conexion.Open();

                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        lista.Add(LeerProveedor(lector));
                    }
                }
            }

            return lista;
        }

        public ObservableCollection<DetallePedido> ListarDetallesPedidosPorFecha(System.DateTime fechaInicio, System.DateTime fechaFin)
        {
            var lista = new ObservableCollection<DetallePedido>();

            using (var conexion = new SqlConnection(_connectionString))
            using (var comando = new SqlCommand("sp_ListarDetallesPedidosPorFecha", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                comando.Parameters.AddWithValue("@FechaFin", fechaFin);
                conexion.Open();

                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        lista.Add(new DetallePedido
                        {
                            IdPedido = lector.GetInt32(lector.GetOrdinal("idpedido")),
                            IdProducto = lector.GetInt32(lector.GetOrdinal("idproducto")),
                            PrecioUnidad = lector.IsDBNull(lector.GetOrdinal("preciounidad")) ? 0 : lector.GetDecimal(lector.GetOrdinal("preciounidad")),
                            Cantidad = lector.IsDBNull(lector.GetOrdinal("cantidad")) ? (short)0 : lector.GetInt16(lector.GetOrdinal("cantidad")),
                            Descuento = lector.IsDBNull(lector.GetOrdinal("descuento")) ? 0 : lector.GetFloat(lector.GetOrdinal("descuento")),
                            FechaPedido = lector.GetDateTime(lector.GetOrdinal("FechaPedido"))
                        });
                    }
                }
            }

            return lista;
        }

        private Proveedor LeerProveedor(SqlDataReader lector)
        {
            return new Proveedor
            {
                IdProveedor = lector.GetInt32(lector.GetOrdinal("idProveedor")),
                NombreCompania = lector.GetString(lector.GetOrdinal("NombreCompañia")),
                NombreContacto = lector.IsDBNull(lector.GetOrdinal("NombreContacto")) ? "" : lector.GetString(lector.GetOrdinal("NombreContacto")),
                CargoContacto = lector.IsDBNull(lector.GetOrdinal("CargoContacto")) ? "" : lector.GetString(lector.GetOrdinal("CargoContacto")),
                Direccion = lector.IsDBNull(lector.GetOrdinal("Direccion")) ? "" : lector.GetString(lector.GetOrdinal("Direccion")),
                Ciudad = lector.IsDBNull(lector.GetOrdinal("Ciudad")) ? "" : lector.GetString(lector.GetOrdinal("Ciudad")),
                Region = lector.IsDBNull(lector.GetOrdinal("Region")) ? "" : lector.GetString(lector.GetOrdinal("Region")),
                CodPostal = lector.IsDBNull(lector.GetOrdinal("CodPostal")) ? "" : lector.GetString(lector.GetOrdinal("CodPostal")),
                Pais = lector.IsDBNull(lector.GetOrdinal("Pais")) ? "" : lector.GetString(lector.GetOrdinal("Pais")),
                Telefono = lector.IsDBNull(lector.GetOrdinal("Telefono")) ? "" : lector.GetString(lector.GetOrdinal("Telefono")),
                Fax = lector.IsDBNull(lector.GetOrdinal("Fax")) ? "" : lector.GetString(lector.GetOrdinal("Fax"))
            };
        }
    }
}