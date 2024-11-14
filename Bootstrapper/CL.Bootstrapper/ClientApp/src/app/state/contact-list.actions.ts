import { AddContactRequest, EditContactRequest } from "src/api/services"

export namespace ContactList {

    export class Initialize {
        static readonly type = "[Contact List] Initialize"
    }

    export class FetchContacts {
        static readonly type = "[Contact List] Fetch Contacts"
    }

    export class FetchCategories {
        static readonly type = "[Contact List] Fetch Categories"
    }

    export class FetchContactDetails {
        static readonly type = "[Contact List] Fetch Contact Details"
        constructor(public payload: { contactId: number }) { }
    }

    export class AddContact {
        static readonly type = "[Contact List] Add Contact"
        constructor(public payload: { dto: AddContactRequest }) { }
    }

    export class EditContact {
        static readonly type = "[Contact List] Edit Contact"
        constructor(public payload: { dto: EditContactRequest }) { }
    }

    export class RemoveContact {
        static readonly type = "[Contact List] Remove Contact"
        constructor(public payload: { contactId: number }) { }
    }

    export class Login {
        static readonly type = "[Contact List] Login"
        constructor(public payload: { email: string, password: string }) { }
    }

    export class Logout {
        static readonly type = "[Contact List] Logout"
    }

    export class CleanContactDetailData {
        static readonly type = "[Contact List] Clean Contact Detail Data"
    }
}
