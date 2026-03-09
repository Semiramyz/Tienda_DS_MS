import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../services/api.service';
import { ProductoResponse, RegistrarCompraRequest, ProveedorResponse } from '../../models/api.models';

@Component({
  selector: 'app-productos',
  standalone: false,
  templateUrl: './productos.html',
  styleUrl: './productos.css'
})
export class ProductosComponent implements OnInit {
  productos: ProductoResponse[] = [];
  proveedores: ProveedorResponse[] = [];
  mostrarForm = false;
  nuevo: RegistrarCompraRequest = { nombre: '', precioCompra: 0, precioVenta: 0, cantidad: 1, proveedorId: 0 };

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.cargar();
    this.api.getProveedores().subscribe(data => this.proveedores = data);
  }

  cargar(): void {
    this.api.getProductos().subscribe(data => this.productos = data);
  }

  registrarCompra(): void {
    this.api.registrarCompra(this.nuevo).subscribe({
      next: () => {
        this.mostrarForm = false;
        this.nuevo = { nombre: '', precioCompra: 0, precioVenta: 0, cantidad: 1, proveedorId: 0 };
        this.cargar();
      },
      error: (e) => alert('Error: ' + (e.error?.message || 'No se pudo registrar'))
    });
  }
}
