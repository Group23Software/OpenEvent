export interface Category
{
  Id: string;
  Name: string;
}

export interface CategoryViewModel
{
  Id: string;
  Name: string;
}

export interface PopularCategoryViewModel extends CategoryViewModel
{
  Score: number;
}
