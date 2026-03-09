import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  ClienteResponse, CrearClienteRequest,
  ProveedorResponse, CrearProveedorRequest,
  ProductoResponse, RegistrarCompraRequest,
  VentaResponse, VentaCompletaResponse, CrearVentaRequest,
  FacturaResponse,
  MovimientoResponse, ResumenDiarioResponse
} from '../models/api.models';

@Injectable({ providedIn: 'root' })
export class ApiService {
  constructor(private http: HttpClient) {}

  // --- Clientes ---
  getClientes(): Observable<ClienteResponse[]> {
    return this.http.get<ClienteResponse[]>('/api/clientes');
  }
  crearCliente(data: CrearClienteRequest): Observable<ClienteResponse> {
    return this.http.post<ClienteResponse>('/api/clientes', data);
  }
  buscarClientes(termino: string): Observable<ClienteResponse[]> {
    return this.http.get<ClienteResponse[]>(`/api/clientes/buscar?termino=${termino}`);
  }

  // --- Proveedores ---
  getProveedores(): Observable<ProveedorResponse[]> {
    return this.http.get<ProveedorResponse[]>('/api/proveedores');
  }
  crearProveedor(data: CrearProveedorRequest): Observable<ProveedorResponse> {
    return this.http.post<ProveedorResponse>('/api/proveedores', data);
  }

  // --- Productos (Inventario + Compras) ---
  getProductos(): Observable<ProductoResponse[]> {
    return this.http.get<ProductoResponse[]>('/api/productos');
  }
  registrarCompra(data: RegistrarCompraRequest): Observable<ProductoResponse> {
    return this.http.post<ProductoResponse>('/api/productos/compra', data);
  }

  // --- Ventas ---
  getVentas(): Observable<VentaResponse[]> {
    return this.http.get<VentaResponse[]>('/api/ventas');
  }
  crearVenta(data: CrearVentaRequest): Observable<VentaCompletaResponse> {
    return this.http.post<VentaCompletaResponse>('/api/ventas', data);
  }
  getVenta(id: number): Observable<VentaCompletaResponse> {
    return this.http.get<VentaCompletaResponse>(`/api/ventas/${id}`);
  }

  // --- Facturas ---
  getFacturas(): Observable<FacturaResponse[]> {
    return this.http.get<FacturaResponse[]>('/api/facturas');
  }
  getFactura(id: number): Observable<FacturaResponse> {
    return this.http.get<FacturaResponse>(`/api/facturas/${id}`);
  }
  getFacturasPorCliente(clienteId: number): Observable<FacturaResponse[]> {
    return this.http.get<FacturaResponse[]>(`/api/facturas/cliente/${clienteId}`);
  }

  // --- Contabilidad ---
  getMovimientos(): Observable<MovimientoResponse[]> {
    return this.http.get<MovimientoResponse[]>('/api/contabilidad/movimientos');
  }
  getResumen(desde?: string, hasta?: string): Observable<ResumenDiarioResponse[]> {
    let url = '/api/contabilidad/resumen';
    const params: string[] = [];
    if (desde) params.push(`desde=${desde}`);
    if (hasta) params.push(`hasta=${hasta}`);
    if (params.length) url += '?' + params.join('&');
    return this.http.get<ResumenDiarioResponse[]>(url);
  }
}
