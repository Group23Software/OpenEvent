import { TestBed } from '@angular/core/testing';

import { PopularityService } from './popularity.service';

describe('PopularityService', () => {
  let service: PopularityService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PopularityService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
