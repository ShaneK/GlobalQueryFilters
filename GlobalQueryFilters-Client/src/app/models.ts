export interface Pizza {
  id: number;
  name: string;
  active: boolean;
}

export interface PizzaResult {
  success: boolean;
  pizzas?: Array<Pizza>;
}

export interface Topping {
  id: number;
  name: string;
  active: boolean;
}

export interface ToppingResult {
  success: boolean;
  toppings?: Array<Topping>;
}
