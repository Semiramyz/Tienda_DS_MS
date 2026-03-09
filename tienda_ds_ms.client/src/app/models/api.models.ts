export interface LoginRequest {
  email: string;
  password: string;
}

export interface TokenResponse {
  token: string;
  rol: string;
  expira: string;
}

export interface UsuarioResponse {
  id: number;
  nombre: string;
  email: string;
  rol: string;
  activo: boolean;
}

export interface RegistroRequest {
  nombre: string;
  email: string;
  password: string;
  rol: string;
}

export interface ClienteResponse {
  id: number;
  nombre: string;
  nit: string;
  email?: string;
  telefono?: string;
  direccion?: string;
}

export interface CrearClienteRequest {
  nombre: string;
  nit: string;
  email?: string;
  telefono?: string;
  direccion?: string;
}

export interface ProveedorResponse {
  id: number;
  nombreEmpresa: string;
  contacto?: string;
  nit: string;
  email?: string;
  telefono?: string;
  direccion?: string;
}

export interface CrearProveedorRequest {
  nombreEmpresa: string;
  contacto?: string;
  nit: string;
  email?: string;
  telefono?: string;
  direccion?: string;
}

export interface ProductoResponse {
  id: number;
  nombre: string;
  descripcion?: string;
  precioCompra: number;
  precioVenta: number;
  stock: number;
  proveedorId: number;
  activo: boolean;
}

export interface RegistrarCompraRequest {
  nombre: string;
  descripcion?: string;
  precioCompra: number;
  precioVenta: number;
  cantidad: number;
  proveedorId: number;
}

export interface VentaResponse {
  id: number;
  clienteId: number;
  nombreCliente: string;
  fecha: string;
  subtotal: number;
  impuesto: number;
  total: number;
  estado: string;
}

export interface VentaDetalleResponse {
  id: number;
  productoId: number;
  nombreProducto: string;
  cantidad: number;
  precioUnitario: number;
  subtotal: number;
}

export interface FacturaResponse {
  id: number;
  numeroFactura: string;
  ventaId: number;
  clienteId: number;
  nombreCliente: string;
  nitCliente: string;
  fechaEmision: string;
  subtotal: number;
  impuesto: number;
  total: number;
  estado: string;
}

export interface VentaCompletaResponse {
  venta: VentaResponse;
  detalles: VentaDetalleResponse[];
  factura: FacturaResponse;
}

export interface CrearVentaRequest {
  clienteId: number;
  items: ItemVenta[];
}

export interface ItemVenta {
  productoId: number;
  cantidad: number;
}

export interface MovimientoResponse {
  id: number;
  tipo: string;
  referenciaId: number;
  descripcion: string;
  monto: number;
  fecha: string;
}

export interface ResumenDiarioResponse {
  fecha: string;
  totalCompras: number;
  totalVentas: number;
  ganancia: number;
}
