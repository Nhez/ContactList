import { Component, OnDestroy, OnInit } from '@angular/core';
import { EditContactRequest, ContactCategoryDto, ContactSubcategoryRequest, ContactDetailsDto } from 'src/api/services';
import { ContactListState } from '../state/contact-list.state';
import { Observable, Subject, takeUntil } from 'rxjs';
import { Select, Store } from '@ngxs/store';
import { Router } from '@angular/router';
import { CategoryWithSubcategories } from '../state/contact-list.model';
import { ContactList } from '../state/contact-list.actions';

@Component({
  selector: 'app-edit-contact',
  templateUrl: './edit-contact.component.html',
  styleUrls: ['./edit-contact.component.css']
})

export class EditContactComponent implements OnInit, OnDestroy {
  private _destroy$: Subject<any> = new Subject();

  public Id: number;
  public Name: string = "";
  public Surname: string = "";
  public Email: string = "";
  public Category: CategoryWithSubcategories;
  public Subcategory: ContactCategoryDto;
  public OtherSubcategory: string  = "";
  public Phone: string = "";
  public DateOfBirth: Date;


  @Select(ContactListState.getContactDetails)
  private contactDetails$: Observable<ContactDetailsDto>;

  @Select(ContactListState.getContactCategories)
  private categories$: Observable<ContactCategoryDto[]>;

  public contactDetails: ContactDetailsDto;
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

    this.contactDetails$
      .pipe(takeUntil(this._destroy$))
      .subscribe((result) => {
        this.Id = result.contactId as number;
        this.Name = result.name as string;
        this.Surname = result.surname as string;
        this.Email = result.email as string;
        this.Category = result.category as CategoryWithSubcategories;
        this.Subcategory = result.subCategory as ContactCategoryDto;
        this.Phone = result.phone as string;
        this.DateOfBirth = result.dateOfBirth as Date;
      });
  };

  public ngOnDestroy() {
    this._destroy$.next(1);
    this._destroy$.complete();
  }

  public onEditContactClicked()
  {
    const subcategoryRequest = {
      id: this.Subcategory?.id,
      value: this.OtherSubcategory
    } as ContactSubcategoryRequest

    const dto = {
      contactId: this.Id,
      name: this.Name,
      surname: this.Surname,
      email: this.Email,
      categoryId: this.Category.id,
      subCategory: subcategoryRequest,
      phone: this.Phone,
      dateOfBirth: this.DateOfBirth
    } as EditContactRequest

    this.store.dispatch(new ContactList.EditContact({dto}));
    this.router.navigate(['/', 'contacts']);
  }
}
