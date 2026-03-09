import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './pages/login/login';
import { DashboardComponent } from './pages/dashboard/dashboard';
import { ClientesComponent } from './pages/clientes/clientes';
import { ProveedoresComponent } from './pages/proveedores/proveedores';
import { ProductosComponent } from './pages/productos/productos';
import { VentasComponent } from './pages/ventas/ventas';
import { FacturasComponent } from './pages/facturas/facturas';
import { ContabilidadComponent } from './pages/contabilidad/contabilidad';
import { AuthGuard } from './guards/auth.guard';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  {
    path: 'dashboard',
    component: DashboardComponent,
    canActivate: [AuthGuard],
    children: [
      { path: 'clientes', component: ClientesComponent },
      { path: 'proveedores', component: ProveedoresComponent },
      { path: 'productos', component: ProductosComponent },
      { path: 'ventas', component: VentasComponent },
      { path: 'facturas', component: FacturasComponent },
      { path: 'contabilidad', component: ContabilidadComponent },
      { path: '', redirectTo: 'clientes', pathMatch: 'full' }
    ]
  },
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: '**', redirectTo: '/login' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
