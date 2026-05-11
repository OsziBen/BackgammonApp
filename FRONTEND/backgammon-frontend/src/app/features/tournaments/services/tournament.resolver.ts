import { inject } from '@angular/core';
import { ActivatedRouteSnapshot } from '@angular/router';
import { TournamentsApiService } from '../services/tournaments-api.service';

export const tournamentResolver = (route: ActivatedRouteSnapshot) => {
  const id = route.paramMap.get('tournamentId')!;

  return inject(TournamentsApiService).getTournamentById(id);
};
