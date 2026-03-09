import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { LoginRequest, TokenResponse } from '../models/api.models';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private tokenKey = 'auth_token';
  private rolKey = 'auth_rol';
  private loggedIn = new BehaviorSubject<boolean>(this.hasToken());

  isLoggedIn$ = this.loggedIn.asObservable();

  constructor(private http: HttpClient) {}

  login(data: LoginRequest): Observable<TokenResponse> {
    return this.http.post<TokenResponse>('/api/auth/login', data).pipe(
      tap((res) => {
        localStorage.setItem(this.tokenKey, res.token);
        localStorage.setItem(this.rolKey, res.rol);
        this.loggedIn.next(true);
      })
    );
  }

  logout(): void {
    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem(this.rolKey);
    this.loggedIn.next(false);
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  getRol(): string | null {
    return localStorage.getItem(this.rolKey);
  }

  hasToken(): boolean {
    return !!localStorage.getItem(this.tokenKey);
  }
}
