import { Component, OnDestroy, OnInit } from '@angular/core';
import { AddContactRequest, ContactCategoryDto, ContactSubcategoryRequest } from 'src/api/services';
import { ContactListState } from '../state/contact-list.state';
import { Observable, Subject, takeUntil } from 'rxjs';
import { Select, Store } from '@ngxs/store';
import { Router } from '@angular/router';
import { CategoryWithSubcategories } from '../state/contact-list.model';
import { ContactList } from '../state/contact-list.actions';

@Component({
  selector: 'app-add-contact',
  templateUrl: './add-contact.component.html',
  styleUrls: ['./add-contact.component.css']
})

export class AddContactComponent implements OnInit, OnDestroy {
  private _destroy$: Subject<any> = new Subject();

  public Name: string = "";
  public Surname: string = "";
  public Email: string = "";
  public Category: CategoryWithSubcategories;
  public Subcategory: ContactCategoryDto;
  public OtherSubcategory: string  = "";
  public Phone: string = "";
  public DateOfBirth: Date;

  @Select(ContactListState.getContactCategories)
  private categories$: Observable<ContactCategoryDto[]>;

  public categories: CategoryWithSubcategories[];

  constructor(
    private readonly store: Store,
    private readonly router: Router
  ) {}

  public ngOnInit(): void {
    this.categories$
      .pipe(takeUntil(this._destroy$))
      .subscribe((result) => {
        let mainCategories = result
          .filter(category => category.parentCategoryId === null || category.parentCategoryId === 0)
          .map(category => category as CategoryWithSubcategories);

        mainCategories
          .forEach(mainCategory => mainCategory.subcategories = result
              .filter(category => category.parentCategoryId === mainCategory.id));

        this.categories = mainCategories;
      });
  };

  public ngOnDestroy() {
    this._destroy$.next(1);
    this._destroy$.complete();
  }

  public onAddContactClicked()
  {
    const subcategoryRequest = {
      id: this.Subcategory?.id,
      value: this.OtherSubcategory
    } as ContactSubcategoryRequest

    const dto = {
      name: this.Name,
      surname: this.Surname,
      email: this.Email,
      categoryId: this.Category.id,
      subCategory: subcategoryRequest,
      phone: this.Phone,
      dateOfBirth: this.DateOfBirth
    } as AddContactRequest

    this.store.dispatch(new ContactList.AddContact({dto}));
    this.router.navigate(['/', 'contacts']);
  }
}
