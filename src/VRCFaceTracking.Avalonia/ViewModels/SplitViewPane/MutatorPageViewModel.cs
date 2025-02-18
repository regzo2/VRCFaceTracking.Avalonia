using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using VRCFaceTracking.Core.Params.Data;
using VRCFaceTracking.Core.Params.Data.Mutation;

namespace VRCFaceTracking.Avalonia.ViewModels.SplitViewPane;

public partial class MutatorPageViewModel : ViewModelBase
{
    private readonly UnifiedTrackingMutator _trackingMutator;

    public ObservableCollection<TrackingMutation> Mutations { get; }

    public MutatorPageViewModel()
    {
        _trackingMutator = Ioc.Default.GetService<UnifiedTrackingMutator>();
        Mutations = _trackingMutator._mutations;
    }
}
