import { Component, OnDestroy, OnInit } from '@angular/core';
import { Select, Store } from '@ngxs/store';
import { Observable, Subject, takeUntil } from 'rxjs';
import { ContactList } from '../state/contact-list.actions';
import { ContactListState } from '../state/contact-list.state';
import { ContactDto } from 'src/api/services';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})

export class HomeComponent implements OnInit, OnDestroy {

  private _destroy$: Subject<any> = new Subject();

  @Select(ContactListState.getContacts)
  private contacts$: Observable<ContactDto[]>;

  @Select(ContactListState.isLoggedIn)
  private isLoggedIn$: Observable<boolean>;

  public isLoggedIn: boolean;

  public contacts: ContactDto[];

  constructor(
    private readonly store: Store,
    private readonly router: Router,
  ) {}

  public ngOnInit(): void {
    this.contacts$
      .pipe(takeUntil(this._destroy$))
      .subscribe((result) => (this.contacts = result));

    this.isLoggedIn$
      .pipe(takeUntil(this._destroy$))
      .subscribe((result) => (this.isLoggedIn = result));
  }

  public ngOnDestroy() {
    this._destroy$.next(1);
    this._destroy$.complete();
  }

  public onDetailsClicked(id?: number)
  {
    const contactId = id ?? 0;

    this.store.dispatch(new ContactList.FetchContactDetails({ contactId }));
    this.router.navigate(['/', 'contacts', contactId]);
  }

  public onAddClicked()
  {
    this.router.navigate(['/', 'contacts', 'add']);
  }

  public onEditClicked(id?: number)
  {
    const contactId = id ?? 0;

    this.store.dispatch(new ContactList.FetchContactDetails({ contactId }));
    this.router.navigate(['/', 'contacts', 'edit']);
  }

  public onDeleteClicked(id? : number)
  {
    const contactId = id ?? 0;

    this.store.dispatch(new ContactList.RemoveContact({ contactId }));
  }
}
