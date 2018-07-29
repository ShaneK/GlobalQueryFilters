import {Component, OnInit} from '@angular/core';
import {AppService} from './app.service';
import {Pizza, Topping} from './models';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  public pizzaList: Array<Pizza> = [];
  public toppingList: Array<Topping> = [];
  public newPizza: Pizza = <Pizza> {};
  public newTopping: Topping = <Topping> {};

  constructor(
    private _appService: AppService
  ) {}

  async ngOnInit(): Promise<void> {
    this.pizzaList = (await this._appService.getPizzas().toPromise()).pizzas;
    this.toppingList = (await this._appService.getToppings().toPromise()).toppings;
    console.log('Pizza list:', this.pizzaList);
  }

  async savePizza(): Promise<void> {
    if (!this.newPizza.name) {
      return;
    }

    this.pizzaList = (await this._appService.savePizza(this.newPizza).toPromise()).pizzas;
    this.newPizza = <Pizza> {};
  }

  async deletePizza(pizza: Pizza): Promise<void> {
    if (!pizza || !pizza.active) {
      return;
    }

    const confirmation = confirm(`Are you sure you wanna delete ${pizza.name}?`);
    if (!confirmation) {
      return;
    }

    this.pizzaList = (await this._appService.deletePizza(pizza).toPromise()).pizzas;
  }

  async saveTopping(): Promise<void> {
    if (!this.newTopping.name) {
      return;
    }

    this.toppingList = (await this._appService.saveTopping(this.newTopping).toPromise()).toppings;
    this.newTopping = <Topping> {};
  }

  async deleteTopping(topping: Topping): Promise<void> {
    if (!topping || !topping.active) {
      return;
    }

    const confirmation = confirm(`Are you sure you wanna delete ${topping.name}?`);
    if (!confirmation) {
      return;
    }

    this.toppingList = (await this._appService.deleteTopping(topping).toPromise()).toppings;
  }
}
