import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../services/api.service';
import { FacturaResponse } from '../../models/api.models';

@Component({
  selector: 'app-facturas',
  standalone: false,
  templateUrl: './facturas.html',
  styleUrl: './facturas.css'
})
export class FacturasComponent implements OnInit {
  facturas: FacturaResponse[] = [];

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.api.getFacturas().subscribe(data => this.facturas = data);
  }
}
