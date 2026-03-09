import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../services/api.service';
import { ClienteResponse, CrearClienteRequest } from '../../models/api.models';

@Component({
  selector: 'app-clientes',
  standalone: false,
  templateUrl: './clientes.html',
  styleUrl: './clientes.css'
})
export class ClientesComponent implements OnInit {
  clientes: ClienteResponse[] = [];
  busqueda = '';
  mostrarForm = false;
  nuevo: CrearClienteRequest = { nombre: '', nit: '' };

  constructor(private api: ApiService) {}

  ngOnInit(): void { this.cargar(); }

  cargar(): void {
    this.api.getClientes().subscribe(data => this.clientes = data);
  }

  buscar(): void {
    if (!this.busqueda.trim()) { this.cargar(); return; }
    this.api.buscarClientes(this.busqueda).subscribe(data => this.clientes = data);
  }

  crear(): void {
    this.api.crearCliente(this.nuevo).subscribe({
      next: () => { this.mostrarForm = false; this.nuevo = { nombre: '', nit: '' }; this.cargar(); },
      error: (e) => alert('Error: ' + (e.error?.message || 'No se pudo crear'))
    });
  }
}
