import { provideHttpClient, withInterceptorsFromDi, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing-module';
import { App } from './app';
import { AuthInterceptor } from './interceptors/auth.interceptor';

import { LoginComponent } from './pages/login/login';
import { DashboardComponent } from './pages/dashboard/dashboard';
import { ClientesComponent } from './pages/clientes/clientes';
import { ProveedoresComponent } from './pages/proveedores/proveedores';
import { ProductosComponent } from './pages/productos/productos';
import { VentasComponent } from './pages/ventas/ventas';
import { FacturasComponent } from './pages/facturas/facturas';
import { ContabilidadComponent } from './pages/contabilidad/contabilidad';

@NgModule({
  declarations: [
    App,
    LoginComponent,
    DashboardComponent,
    ClientesComponent,
    ProveedoresComponent,
    ProductosComponent,
    VentasComponent,
    FacturasComponent,
    ContabilidadComponent
  ],
  imports: [
    BrowserModule, FormsModule,
    AppRoutingModule
  ],
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideHttpClient(withInterceptorsFromDi()),
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
  ],
  bootstrap: [App]
})
export class AppModule { }
