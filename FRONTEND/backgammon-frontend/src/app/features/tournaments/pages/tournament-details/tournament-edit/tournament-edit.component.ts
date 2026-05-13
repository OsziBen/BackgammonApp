import { CommonModule } from '@angular/common';
import { Component, OnInit, signal } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { firstValueFrom } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { TournamentBaseResponse } from '../../../models/api/responses/tournament-base.response';
import { UpdateTournamentRequest } from '../../../models/api/requests/update-tournament.request';
import { TournamentsApiService } from '../../../services/tournaments-api.service';
import { RulesTemplateResponse } from '../../../../rules-template/models/api/responses/rules-template.response';
import { RulesTemplatesApiService } from '../../../../rules-template/services/rules-templates-api.service';

type TournamentType = 'Swiss' | 'SingleElimination' | 'RoundRobin';
type TournamentVisibility = 'Public' | 'Private';
type TournamentStatus =
  | 'Planned'
  | 'RegistrationOpen'
  | 'Ongoing'
  | 'Finished'
  | 'Cancelled';

@Component({
  selector: 'app-tournament-edit',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './tournament-edit.component.html',
  styleUrls: ['./tournament-edit.component.css'],
})
export class TournamentEditComponent implements OnInit {
  readonly loading = signal(false);

  tournament!: TournamentBaseResponse;

  rulesTemplates: RulesTemplateResponse[] = [];

  typeOptions: TournamentType[] = ['Swiss', 'SingleElimination', 'RoundRobin'];

  visibilityOptions: TournamentVisibility[] = ['Public', 'Private'];

  statusOptions: TournamentStatus[] = [
    'Planned',
    'RegistrationOpen',
    'Ongoing',
    'Finished',
    'Cancelled',
  ];

  form!: FormGroup;

  constructor(
    private readonly fb: FormBuilder,
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly api: TournamentsApiService,
    private readonly rulesTemplatesApi: RulesTemplatesApiService,
    private readonly toastr: ToastrService,
  ) {}

  ngOnInit(): void {
    this.tournament = this.route.parent!.snapshot.data[
      'tournament'
    ] as TournamentBaseResponse;

    this.loadRulesTemplates();
  }

  private loadRulesTemplates(): void {
    this.rulesTemplatesApi.getAllTemplates().subscribe({
      next: (data) => {
        this.rulesTemplates = data;

        this.initForm(data);
      },
      error: (err) => {
        console.error('Failed to load rules templates', err);
      },
    });
  }

  private initForm(templates: RulesTemplateResponse[]): void {
    this.form = this.fb.group({
      name: [this.tournament.name, Validators.required],
      description: [this.tournament.description ?? ''],

      type: [this.tournament.type, Validators.required],
      visibility: [this.tournament.visibility, Validators.required],

      status: [
        this.normalizeStatus(this.tournament.status),
        Validators.required,
      ],

      maxParticipants: [this.tournament.maxParticipants, Validators.required],

      startDate: [
        this.toInputDate(this.tournament.startDate),
        Validators.required,
      ],

      endDate: [this.toInputDate(this.tournament.endDate), Validators.required],

      deadline: [
        this.toInputDate(this.tournament.deadline),
        Validators.required,
      ],

      rulesTemplateId: [
        this.tournament.rulesTemplate?.id ??
          (templates.length > 0 ? templates[0].id : ''),
        Validators.required,
      ],
    });
  }

  async onSubmit(): Promise<void> {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.loading.set(true);

    try {
      const request: UpdateTournamentRequest = {
        name: this.form.value.name!,
        description: this.form.value.description ?? '',
        type: this.form.value.type!,
        visibility: this.form.value.visibility!,
        status: this.form.value.status!,
        maxParticipants: this.form.value.maxParticipants!,
        startDate: this.form.value.startDate!,
        endDate: this.form.value.endDate!,
        deadline: this.form.value.deadline!,
        rulesTemplateId: this.form.value.rulesTemplateId!,
      };

      const updated = await firstValueFrom(
        this.api.updateTournament(this.tournament.id, request),
      );

      this.toastr.success('Tournament updated');

      await this.router.navigateByUrl('/', { skipLocationChange: true });

      await this.router.navigate([
        '/tournaments',
        this.tournament.id,
        'overview',
      ]);
    } catch (err) {
      console.error(err);
      this.toastr.error('Could not update tournament');
    } finally {
      this.loading.set(false);
    }
  }

  private toInputDate(date: string): string {
    return new Date(date).toISOString().slice(0, 16);
  }

  private normalizeStatus(status: string): TournamentStatus {
    switch (status?.toLowerCase()) {
      case 'planned':
        return 'Planned';
      case 'registrationopen':
        return 'RegistrationOpen';
      case 'ongoing':
        return 'Ongoing';
      case 'finished':
        return 'Finished';
      case 'cancelled':
        return 'Cancelled';
      default:
        return 'Planned';
    }
  }
}
