import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { AddContactComponent } from './add-contact/add-contact.component';
import { EditContactComponent } from './edit-contact/edit-contact.component';
import { ContactDetailsComponent } from './contact-details/contact-details.component';

const routes: Routes = [
    {
        path: '',
        redirectTo: 'contacts',
        pathMatch: 'full',
    },
    {
        path: 'contacts',
        component: HomeComponent
    },
    {
        path: 'contacts/add',
        component: AddContactComponent,
        pathMatch: 'full',
    },
    {
        path: 'contacts/edit/:contactId',
        component: EditContactComponent,
        pathMatch: 'full'
    },
    {
        path: 'contacts/:contactId',
        component: ContactDetailsComponent
    },
    {
        path: '**',
        redirectTo: 'contacts'
    }
  ];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
  })

export class AppRoutingModule {}
