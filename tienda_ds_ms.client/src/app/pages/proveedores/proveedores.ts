import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../services/api.service';
import { ProveedorResponse, CrearProveedorRequest } from '../../models/api.models';

@Component({
  selector: 'app-proveedores',
  standalone: false,
  templateUrl: './proveedores.html',
  styleUrl: './proveedores.css'
})
export class ProveedoresComponent implements OnInit {
  proveedores: ProveedorResponse[] = [];
  mostrarForm = false;
  nuevo: CrearProveedorRequest = { nombreEmpresa: '', nit: '' };

  constructor(private api: ApiService) {}

  ngOnInit(): void { this.cargar(); }

  cargar(): void {
    this.api.getProveedores().subscribe(data => this.proveedores = data);
  }

  crear(): void {
    this.api.crearProveedor(this.nuevo).subscribe({
      next: () => { this.mostrarForm = false; this.nuevo = { nombreEmpresa: '', nit: '' }; this.cargar(); },
      error: (e) => alert('Error: ' + (e.error?.message || 'No se pudo crear'))
    });
  }
}
