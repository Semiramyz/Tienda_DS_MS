import { Component, OnInit, ViewChild, ElementRef, AfterViewChecked } from '@angular/core';
import { ApiService } from '../../services/api.service';
import { MovimientoResponse, ResumenDiarioResponse } from '../../models/api.models';

@Component({
  selector: 'app-contabilidad',
  standalone: false,
  templateUrl: './contabilidad.html',
  styleUrl: './contabilidad.css'
})
export class ContabilidadComponent implements OnInit, AfterViewChecked {
  movimientos: MovimientoResponse[] = [];
  resumen: ResumenDiarioResponse[] = [];
  totalCompras = 0;
  totalVentas = 0;

  private chartsDrawn = false;

  @ViewChild('barChart') barChartRef!: ElementRef<HTMLCanvasElement>;
  @ViewChild('donutChart') donutChartRef!: ElementRef<HTMLCanvasElement>;

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.api.getMovimientos().subscribe(data => {
      this.movimientos = data;
      this.calcTotals();
      this.chartsDrawn = false;
    });
    this.api.getResumen().subscribe(data => {
      this.resumen = data;
      this.chartsDrawn = false;
    });
  }

  ngAfterViewChecked(): void {
    if (!this.chartsDrawn && (this.resumen.length > 0 || this.movimientos.length > 0)) {
      this.chartsDrawn = true;
      setTimeout(() => this.drawCharts(), 0);
    }
  }

  private calcTotals(): void {
    this.totalCompras = this.movimientos
      .filter(m => m.tipo === 'COMPRA')
      .reduce((s, m) => s + m.monto, 0);
    this.totalVentas = this.movimientos
      .filter(m => m.tipo === 'VENTA')
      .reduce((s, m) => s + m.monto, 0);
  }

  private drawCharts(): void {
    if (this.barChartRef && this.resumen.length > 0) {
      this.drawBarChart();
    }
    if (this.donutChartRef && this.movimientos.length > 0) {
      this.drawDonutChart();
    }
  }

  // ==================== GRÁFICO DE BARRAS ====================
  private drawBarChart(): void {
    const canvas = this.barChartRef.nativeElement;
    const ctx = canvas.getContext('2d')!;
    const dpr = window.devicePixelRatio || 1;

    const rect = canvas.parentElement!.getBoundingClientRect();
    const w = rect.width - 32;
    const h = 300;

    canvas.width = w * dpr;
    canvas.height = h * dpr;
    canvas.style.width = w + 'px';
    canvas.style.height = h + 'px';
    ctx.scale(dpr, dpr);

    const navy = '#0a1f3b';
    const accent = '#2563eb';
    const green = '#16a34a';

    const pad = { top: 30, right: 20, bottom: 60, left: 70 };
    const chartW = w - pad.left - pad.right;
    const chartH = h - pad.top - pad.bottom;

    const data = this.resumen.slice(-10);
    const groups = data.length;
    const barsPerGroup = 3;
    const groupWidth = chartW / groups;
    const barWidth = Math.min(groupWidth * 0.22, 28);
    const gap = barWidth * 0.3;

    const allValues = data.flatMap(d => [d.totalCompras, d.totalVentas, Math.abs(d.ganancia)]);
    const maxVal = Math.max(...allValues, 1);

    // Background
    ctx.fillStyle = '#ffffff';
    ctx.fillRect(0, 0, w, h);

    // Grid lines
    ctx.strokeStyle = '#e2e8f0';
    ctx.lineWidth = 1;
    const gridLines = 5;
    for (let i = 0; i <= gridLines; i++) {
      const y = pad.top + (chartH / gridLines) * i;
      ctx.beginPath();
      ctx.moveTo(pad.left, y);
      ctx.lineTo(w - pad.right, y);
      ctx.stroke();

      // Y-axis labels
      const val = maxVal - (maxVal / gridLines) * i;
      ctx.fillStyle = '#475569';
      ctx.font = '11px Inter, sans-serif';
      ctx.textAlign = 'right';
      ctx.fillText('Q' + val.toFixed(0), pad.left - 10, y + 4);
    }

    // Bars
    data.forEach((d, i) => {
      const groupX = pad.left + groupWidth * i + groupWidth / 2;
      const startX = groupX - (barsPerGroup * barWidth + (barsPerGroup - 1) * gap) / 2;

      const values = [d.totalCompras, d.totalVentas, d.ganancia];
      const colors = [navy, accent, green];

      values.forEach((val, j) => {
        const bh = (Math.abs(val) / maxVal) * chartH;
        const x = startX + j * (barWidth + gap);
        const y = pad.top + chartH - bh;

        ctx.fillStyle = colors[j];
        this.roundRect(ctx, x, y, barWidth, bh, 3);
      });

      // X-axis labels
      ctx.fillStyle = '#475569';
      ctx.font = '10px Inter, sans-serif';
      ctx.textAlign = 'center';
      const label = d.fecha.length > 10 ? d.fecha.substring(5) : d.fecha;
      ctx.fillText(label, groupX, h - pad.bottom + 18);
    });

    // Legend
    const legendY = h - 15;
    const legendItems = [
      { label: 'Compras', color: navy },
      { label: 'Ventas', color: accent },
      { label: 'Ganancia', color: green }
    ];
    let legendX = pad.left;
    ctx.font = '11px Inter, sans-serif';
    legendItems.forEach(item => {
      ctx.fillStyle = item.color;
      this.roundRect(ctx, legendX, legendY - 8, 12, 12, 2);
      ctx.fillStyle = '#475569';
      ctx.textAlign = 'left';
      ctx.fillText(item.label, legendX + 16, legendY + 2);
      legendX += ctx.measureText(item.label).width + 36;
    });
  }

  // ==================== GRÁFICO DE DONA ====================
  private drawDonutChart(): void {
    const canvas = this.donutChartRef.nativeElement;
    const ctx = canvas.getContext('2d')!;
    const dpr = window.devicePixelRatio || 1;

    const size = 260;
    canvas.width = size * dpr;
    canvas.height = size * dpr;
    canvas.style.width = size + 'px';
    canvas.style.height = size + 'px';
    ctx.scale(dpr, dpr);

    const cx = size / 2;
    const cy = size / 2;
    const outerR = 110;
    const innerR = 65;

    const total = this.totalCompras + this.totalVentas;
    if (total === 0) return;

    const slices = [
      { value: this.totalCompras, color: '#132d52' },
      { value: this.totalVentas, color: '#2563eb' }
    ];

    // White background
    ctx.fillStyle = '#ffffff';
    ctx.fillRect(0, 0, size, size);

    let startAngle = -Math.PI / 2;
    slices.forEach(slice => {
      const sliceAngle = (slice.value / total) * Math.PI * 2;
      ctx.beginPath();
      ctx.arc(cx, cy, outerR, startAngle, startAngle + sliceAngle);
      ctx.arc(cx, cy, innerR, startAngle + sliceAngle, startAngle, true);
      ctx.closePath();
      ctx.fillStyle = slice.color;
      ctx.fill();
      startAngle += sliceAngle;
    });

    // Center text
    const pct = total > 0 ? ((this.totalVentas / total) * 100).toFixed(0) : '0';
    ctx.fillStyle = '#0a1f3b';
    ctx.font = 'bold 28px Inter, sans-serif';
    ctx.textAlign = 'center';
    ctx.textBaseline = 'middle';
    ctx.fillText(pct + '%', cx, cy - 8);
    ctx.font = '12px Inter, sans-serif';
    ctx.fillStyle = '#475569';
    ctx.fillText('Ventas', cx, cy + 14);
  }

  // ==================== UTILIDAD ====================
  private roundRect(ctx: CanvasRenderingContext2D, x: number, y: number, w: number, h: number, r: number): void {
    ctx.beginPath();
    ctx.moveTo(x + r, y);
    ctx.lineTo(x + w - r, y);
    ctx.quadraticCurveTo(x + w, y, x + w, y + r);
    ctx.lineTo(x + w, y + h);
    ctx.lineTo(x, y + h);
    ctx.lineTo(x, y + r);
    ctx.quadraticCurveTo(x, y, x + r, y);
    ctx.closePath();
    ctx.fill();
  }
}
