import { Action, Selector, State, StateContext } from "@ngxs/store"
import { ContactListStateModel } from "./contact-list.model"
import { Injectable } from "@angular/core"
import { ContactList } from "./contact-list.actions";

import { ContactDto, ContactListService, LoginRequest, UserService } from "src/api/services"
import { patch, updateItem } from "@ngxs/store/operators";

const defaultStateModel = {
  contacts: [],
  contactCategories: [],
  contactDetails: {},
  isLoggedIn: false
} as ContactListStateModel

@State<ContactListStateModel>({
  name: "contactList",
  defaults: defaultStateModel,
})

@Injectable()
export class ContactListState {

  constructor(
    private readonly contactListService: ContactListService,
    private readonly userService: UserService
  ) {}

  @Action([ContactList.Initialize])
  initialize(ctx: StateContext<ContactListStateModel>, action: ContactList.Initialize) {
    const { setState, dispatch } = ctx;

    setState(
      {
        ...defaultStateModel
      }
    );

    dispatch(new ContactList.FetchContacts());
    dispatch(new ContactList.FetchCategories());
  }

  @Action([ContactList.FetchContacts])
  async fetchContacts(ctx: StateContext<ContactListStateModel>, action: ContactList.FetchContacts) {
    const { setState } = ctx;

    const contactsDto = await this.contactListService
      .getContacts()
      .toPromise();

      setState(
        patch({
          contacts: contactsDto
        }));
  }

  @Action([ContactList.FetchCategories])
  async fetchCategories(ctx: StateContext<ContactListStateModel>, action: ContactList.FetchCategories) {
    const { setState } = ctx;

    const categoriesDto = await this.contactListService
      .getCategories()
      .toPromise();

    setState(
      patch({
        contactCategories: categoriesDto
      })
    );
  }

  @Action([ContactList.FetchContactDetails])
  async fetchContactDetails(ctx: StateContext<ContactListStateModel>, action: ContactList.FetchContactDetails) {
    const { setState } = ctx;
    const { contactId } = action.payload

    const contactDetailsDto = await this.contactListService
      .getContactDetails(contactId)
      .toPromise();

    setState(
      patch({
        contactDetails: contactDetailsDto
      })
    );
  }

  @Action([ContactList.AddContact])
  async addContact(ctx: StateContext<ContactListStateModel>, action: ContactList.AddContact) {
    const { getState, setState } = ctx;
    const { contacts } = getState();
    const { dto } = action.payload

    const result = await this.contactListService
      .addContact(dto)
      .toPromise();

    const newContact = {
      id: result?.id,
      name: result?.name,
      surname: result?.surname,
      category: result?.category
    } as ContactDto

    const newContacts = contacts.map(c => {
      return {
        id: c.id,
        name: c.name,
        surname: c.surname,
        category: c.category
      } as ContactDto
    })

    newContacts.push(newContact);

    setState(
      patch({
        contacts: newContacts
      })
    );
  }

  @Action([ContactList.EditContact])
  async editContact(ctx: StateContext<ContactListStateModel>, action: ContactList.EditContact) {
    const { setState } = ctx;
    const { dto } = action.payload

    const result = await this.contactListService
      .editContact(dto)
      .toPromise();

    const editedContact = {
      id: result?.id,
      name: result?.name,
      surname: result?.surname,
      category: result?.category
    } as ContactDto

    setState(
      patch({
        contacts: updateItem<ContactDto>(contact => contact.id === editedContact.id, () => editedContact)
      }));
  }

  @Action([ContactList.RemoveContact])
  async removeContact(ctx: StateContext<ContactListStateModel>, action: ContactList.RemoveContact) {
    const { getState, setState } = ctx;
    const { contacts } = getState();
    const { contactId } = action.payload

    const result = await this.contactListService
      .deleteContact(contactId)
      .toPromise();

    const newContacts = contacts
      .filter(c => c.id !== contactId)
      .map(c => {
        return {
          id: c.id,
          name: c.name,
          surname: c.surname,
          category: c.category
        } as ContactDto
      })

    setState(
      patch({
        contacts: newContacts
      })
    );
  }

  @Action([ContactList.Login])
  async login(ctx: StateContext<ContactListStateModel>, action: ContactList.Login) {
    const { setState } = ctx;
    const { email, password } = action.payload

    const loginRequest = {
      username: email,
      password: password
    } as LoginRequest

    await this.userService.login(loginRequest).toPromise();

    setState(
      patch({
        isLoggedIn: true
      })
    );
  }

  @Action([ContactList.Logout])
  async logout(ctx: StateContext<ContactListStateModel>, action: ContactList.Logout) {
    const { setState } = ctx;

    await this.userService.logout().toPromise();

    setState(
      patch({
        contactDetails: {},
        isLoggedIn: false
      })
    );
  }

  @Action([ContactList.CleanContactDetailData])
  async cleanContactDetailData(ctx: StateContext<ContactListStateModel>, action: ContactList.CleanContactDetailData) {
    const { setState } = ctx;

    setState(
      patch({
        contactDetails: {}
      })
    );
  }

  @Selector()
  static getContacts(state: ContactListStateModel) {
    return state.contacts;
  }

  @Selector()
  static getContactDetails(state: ContactListStateModel) {
    return state.contactDetails;
  }

  @Selector()
  static isLoggedIn(state: ContactListStateModel) {
    return state.isLoggedIn;
  }

  @Selector()
  static getContactCategories(state: ContactListStateModel) {
    return state.contactCategories;
  }
}
