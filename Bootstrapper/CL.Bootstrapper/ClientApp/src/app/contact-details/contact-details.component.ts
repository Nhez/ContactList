import { Component, OnDestroy, OnInit } from '@angular/core';
import { Select, Store } from '@ngxs/store';
import { Observable, Subject, takeUntil } from 'rxjs';
import { ContactListState } from '../state/contact-list.state';
import { ContactDetailsDto } from 'src/api/services';
import { Router } from '@angular/router';
import { ContactList } from '../state/contact-list.actions';


@Component({
  selector: 'contact-details',
  templateUrl: './contact-details.component.html',
  styleUrls: ['./contact-details.component.css']
})

export class ContactDetailsComponent implements OnInit, OnDestroy {

  private _destroy$: Subject<any> = new Subject();

  @Select(ContactListState.getContactDetails)
  private contactDetails$: Observable<ContactDetailsDto>;

  public contactDetails: ContactDetailsDto;

  constructor(
    private readonly store: Store,
    private readonly router: Router
  ) {}

  public ngOnInit(): void {
    this.contactDetails$
      .pipe(takeUntil(this._destroy$))
      .subscribe((result) => (this.contactDetails = result));
  }

  public ngOnDestroy() {
    this._destroy$.next(1);
    this._destroy$.complete();
  }

  public onBackClicked()
  {
    this.store.dispatch(new ContactList.CleanContactDetailData());
    this.router.navigate(['/', 'contacts']);
  }
}
