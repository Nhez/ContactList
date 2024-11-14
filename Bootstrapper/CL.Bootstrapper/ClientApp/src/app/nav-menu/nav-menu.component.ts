import { Component, OnDestroy, OnInit } from '@angular/core';
import { Select, Store } from '@ngxs/store';
import { Observable, Subject, takeUntil } from 'rxjs';
import { ContactListState } from '../state/contact-list.state';
import { ContactList } from '../state/contact-list.actions';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})

export class NavMenuComponent implements OnInit, OnDestroy {
  private _destroy$: Subject<any> = new Subject();

  @Select(ContactListState.isLoggedIn)
  private isLoggedIn$: Observable<boolean>;

  public isLoggedIn: boolean;
  public email: string = "";
  public password: string  = "";

  constructor(
    private readonly store: Store,
    private readonly router: Router
  ) {}

  public ngOnInit(): void {
    this.store.dispatch(new ContactList.Initialize());

    this.isLoggedIn$
      .pipe(takeUntil(this._destroy$))
      .subscribe((result) => (this.isLoggedIn = result));
  }

  public ngOnDestroy() {
    this._destroy$.next(1);
    this._destroy$.complete();
  }

  public onLoginClicked()
  {
    this.store.dispatch(new ContactList.Login({ email: this.email, password: this.password }));
  }

  public onLogoutClicked()
  {
    this.store.dispatch(new ContactList.Logout());
    this.router.navigate(['/', 'contacts']);
  }
}
