import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {Pizza, PizzaResult, Topping, ToppingResult} from './models';

@Injectable({
  providedIn: 'root'
})
export class AppService {
  constructor(
    private _http: HttpClient
  ) {
  }

  public getPizzas(): Observable<PizzaResult> {
    return <Observable<PizzaResult>> this._http.get('/api/pizza');
  }

  public savePizza(pizza: Pizza): Observable<PizzaResult> {
    if (!pizza.id) {
      return <Observable<PizzaResult>> this._http.post('/api/pizza', pizza);
    } else {
      return <Observable<PizzaResult>> this._http.put(`/api/pizza/${pizza.id}`, pizza);
    }
  }

  public deletePizza(pizza: Pizza): Observable<PizzaResult> {
    return <Observable<PizzaResult>> this._http.delete(`/api/pizza/${pizza.id}`);
  }

  public getToppings(): Observable<ToppingResult> {
    return <Observable<ToppingResult>> this._http.get('/api/topping');
  }

  public saveTopping(topping: Topping): Observable<ToppingResult> {
    if (!topping.id) {
      return <Observable<ToppingResult>> this._http.post('/api/topping', topping);
    } else {
      return <Observable<ToppingResult>> this._http.put(`/api/topping/${topping.id}`, topping);
    }
  }

  public deleteTopping(topping: Topping): Observable<ToppingResult> {
    return <Observable<ToppingResult>> this._http.delete(`/api/topping/${topping.id}`);
  }
}
