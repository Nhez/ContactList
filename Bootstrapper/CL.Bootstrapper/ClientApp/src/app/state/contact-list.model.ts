import { ContactCategoryDto, ContactDetailsDto, ContactDto } from "src/api/services";

export interface ContactListStateModel {
  contacts: ContactDto[],
  contactCategories: ContactCategoryDto[],
  contactDetails: ContactDetailsDto,
  isLoggedIn: boolean
}

export interface CategoryWithSubcategories extends ContactCategoryDto {
  subcategories: ContactCategoryDto[]
}