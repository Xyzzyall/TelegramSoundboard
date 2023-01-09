using FluentValidation;
using TelegramSoundboardBot.Frontend.Localization;
using TelegramSoundboardBot.Frontend.Requests;
using TelegramSoundboardBot.Soundboard;

namespace TelegramSoundboardBot.Frontend.Validators;

public class AddSoundValidator : AbstractValidator<AddSoundRequest>
{
    public AddSoundValidator(ILocalizationService localization, ISoundsService soundsService)
    {
        RuleFor(req => req.Context.Update.Message!.Caption)
            .NotEmpty()
            .WithMessage(localization.Localize("SoundNameIsEmpty"))
            //todo regex validation for pattern *folder* *name*
            .MustAsync(async (text, ct) => !await soundsService.SoundNameExistAsync(text!, ct))
            .WithMessage((_, text) => localization.Localize("SoundNameAlreadyExists", text!));
    }
}