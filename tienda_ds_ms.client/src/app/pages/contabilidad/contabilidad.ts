import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../services/api.service';
import { MovimientoResponse, ResumenDiarioResponse } from '../../models/api.models';

@Component({
  selector: 'app-contabilidad',
  standalone: false,
  templateUrl: './contabilidad.html',
  styleUrl: './contabilidad.css'
})
export class ContabilidadComponent implements OnInit {
  movimientos: MovimientoResponse[] = [];
  resumen: ResumenDiarioResponse[] = [];

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.api.getMovimientos().subscribe(data => this.movimientos = data);
    this.api.getResumen().subscribe(data => this.resumen = data);
  }
}
