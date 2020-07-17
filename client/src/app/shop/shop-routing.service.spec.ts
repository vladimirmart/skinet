import { TestBed } from '@angular/core/testing';

import { ShopRoutingService } from './shop-routing.service';

describe('ShopRoutingService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ShopRoutingService = TestBed.get(ShopRoutingService);
    expect(service).toBeTruthy();
  });
});
