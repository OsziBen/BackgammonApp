import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { GameSessionApiService } from '../../api/game-session-api.service';
import { GameSessionSettings } from '../../api/models/game-session-settings.model';

@Component({
  selector: 'app-create-game-session',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './create-game-session.component.html',
})
export class CreateGameSessionComponent {
  form: FormGroup;
  showForm = false;

  isSubmitting = false;
  createdSessionId: string | null = null;

  constructor(
    private fb: FormBuilder,
    private api: GameSessionApiService,
  ) {
    this.form = this.fb.group({
      targetPoints: [{ value: 1, disabled: true }],
      doublingCubeEnabled: [true],
      clockEnabled: [{ value: false, disabled: true }],
      matchTimePerPlayerInSeconds: [{ value: null, disabled: true }],
      startOfTurnDelayPerPlayerInSeconds: [{ value: null, disabled: true }],
      crawfordRuleEnabled: [{ value: false, disabled: true }],
    });

    this.form.get('clockEnabled')?.valueChanges.subscribe((enabled) => {
      if (enabled) {
        this.form.get('matchTimePerPlayerInSeconds')?.enable();
        this.form.get('startOfTurnDelayPerPlayerInSeconds')?.enable();
      } else {
        this.form.get('matchTimePerPlayerInSeconds')?.disable();
        this.form.get('startOfTurnDelayPerPlayerInSeconds')?.disable();
      }
    });
  }

  toggleForm(): void {
    this.showForm = !this.showForm;
    if (!this.showForm) {
      this.resetForm();
    }
  }

  resetForm(): void {
    this.createdSessionId = null;
    this.form.reset({
      targetPoints: 1,
      doublingCubeEnabled: true,
      clockEnabled: false,
      matchTimePerPlayerInSeconds: null,
      startOfTurnDelayPerPlayerInSeconds: null,
      crawfordRuleEnabled: false,
    });
    this.form.get('clockEnabled')?.disable();
    this.form.get('matchTimePerPlayerInSeconds')?.disable();
    this.form.get('startOfTurnDelayPerPlayerInSeconds')?.disable();
    this.form.get('crawfordRuleEnabled')?.disable();
  }

  // kell-e?
  back(): void {
    this.createdSessionId = null;
    this.form.reset({
      targetPoints: 3,
      doublingCubeEnabled: true,
      clockEnabled: false,
      crawfordRuleEnabled: false,
    });
  }
  //

  create(): void {
    if (this.form.invalid || this.isSubmitting) {
      return;
    }

    this.isSubmitting = true;

    const settings: GameSessionSettings = this.form.getRawValue();

    this.api
      .createSession({
        hostPlayerId: '11111111-1111-1111-1111-111111111111',
        settings,
      })
      .subscribe({
        next: (sessionId) => {
          this.createdSessionId = sessionId;
          this.isSubmitting = false;
        },
        error: (err) => {
          console.error('Create session failed', err);
          this.isSubmitting = false;
        },
      });
  }
}
