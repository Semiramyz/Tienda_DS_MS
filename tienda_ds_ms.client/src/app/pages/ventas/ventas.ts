import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../services/api.service';
import {
  ClienteResponse, ProductoResponse,
  CrearVentaRequest, ItemVenta, VentaResponse, VentaCompletaResponse
} from '../../models/api.models';

@Component({
  selector: 'app-ventas',
  standalone: false,
  templateUrl: './ventas.html',
  styleUrl: './ventas.css'
})
export class VentasComponent implements OnInit {
  ventas: VentaResponse[] = [];
  clientes: ClienteResponse[] = [];
  productos: ProductoResponse[] = [];
  mostrarForm = false;
  resultado: VentaCompletaResponse | null = null;

  clienteId = 0;
  items: ItemVenta[] = [];
  nuevoItem: ItemVenta = { productoId: 0, cantidad: 1 };
  error = '';

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.cargar();
    this.api.getClientes().subscribe(data => this.clientes = data);
    this.api.getProductos().subscribe(data => this.productos = data);
  }

  cargar(): void {
    this.api.getVentas().subscribe(data => this.ventas = data);
  }

  agregarItem(): void {
    if (this.nuevoItem.productoId === 0 || this.nuevoItem.cantidad < 1) return;
    this.items.push({ ...this.nuevoItem });
    this.nuevoItem = { productoId: 0, cantidad: 1 };
  }

  quitarItem(index: number): void {
    this.items.splice(index, 1);
  }

  getNombreProducto(id: number): string {
    return this.productos.find(p => p.id === id)?.nombre || '?';
  }

  crearVenta(): void {
    this.error = '';
    if (this.clienteId === 0 || this.items.length === 0) {
      this.error = 'Selecciona un cliente y al menos un producto';
      return;
    }
    const req: CrearVentaRequest = { clienteId: this.clienteId, items: this.items };
    this.api.crearVenta(req).subscribe({
      next: (res) => {
        this.resultado = res;
        this.mostrarForm = false;
        this.items = [];
        this.clienteId = 0;
        this.cargar();
        this.api.getProductos().subscribe(data => this.productos = data);
      },
      error: (e) => this.error = e.error?.error || 'Error al crear la venta'
    });
  }
}
