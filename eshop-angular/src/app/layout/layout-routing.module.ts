import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LayoutComponent } from './layout.component';

const routes: Routes = [
  {
    path: '', // http://localhost:4200/
    component: LayoutComponent,
    children: [
        { path: '', redirectTo: 'dashboard', pathMatch: 'prefix' },
        {
            path: 'dashboard',
            loadChildren: () => import('./dashboard/dashboard.module').then(m => m.DashboardModule)
        },
        {
            path: 'cusomer',
            loadChildren: () => import('./cusomer/cusomer.module').then(m => m.CusomerModule)
        },
        {
            path: 'product',
            loadChildren: () => import('./product/product.module').then(m => m.ProductModule)
        },
        {
            path: 'product-category',
            loadChildren: () => import('./product-category/product-category.module').then(m => m.ProductCategoryModule)
        },
        {
            path: 'function',
            loadChildren: () => import('./function/function.module').then(m => m.FunctionModule)
        },
        {
            path: 'role',
            loadChildren: () => import('./role/role.module').then(m => m.RoleModule)
        },
        {
            path: 'user',
            loadChildren: () => import('./user/user.module').then(m => m.UserModule)
        }
    ]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class LayoutRoutingModule {}
