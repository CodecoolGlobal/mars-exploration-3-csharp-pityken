
private void AddToTotalCollectedResources(KeyValuePair<string, int> resource)
{
    if (!TotalCollectedResources.ContainsKey(resource.Key))
    {
        TotalCollectedResources.Add(resource.Key, resource.Value);
    }
    else
    {
        TotalCollectedResources[resource.Key] += resource.Value;
    }
}


private void AssignResourceNodeToRover(Rover rover) //rover has built => run
{
    var mineralResource = ResourceNodes.Count(r => r.HasRoverAssinged == true) == 0
        ? ResourceNodes.First(x => x.Type == "mineral")
        : ResourceNodes.First(x => x.HasRoverAssinged == false);

    rover.AssignResourceNode(mineralResource);
    mineralResource.HasRoverAssinged = true;
}

public Rover? UpdateStatus(int roverCost)
{
    int Minerals = Resources["mineral"];

    if (ResourceNodes.Any(x => !x.HasRoverAssinged) && Minerals >= roverCost)
    {
        return AssembleRover(roverCost, false);
    }

    else if (ExploringRoverNeeded && Minerals >= roverCost)
    {
        var assemblyStatus = AssembleRover(roverCost, true);
        if (assemblyStatus != null)
        {
            ExploringRoverNeeded = false;
        }
        return assemblyStatus;
    }

    CommandCenterStatus = CommandCenterStatus.Idle;
    return null;
}


public bool IsConstructable(int resourceNeeded, int totalResource)
{
    return CommandCenterStatus == CommandCenterStatus.UnderConstruction && resourceNeeded <= totalResource;
}


private List<ResourceNode> GetResourcesInSight(Dictionary<string, HashSet<Coordinate>> discoveredResources)
{
    List<ResourceNode> resources = new List<ResourceNode>();
    foreach (var discResource in discoveredResources)
    {
        foreach (Coordinate coord in AdjacentCoordinates)
        {
            if (discResource.Value.Any(x => x.X == coord.X && x.Y == coord.Y))
            {
                resources.Add(new ResourceNode(discResource.Key, coord, false));
            }
        }
    }
    return resources;
}

private Rover? AssembleRover(int roverCost, bool exploring)
{
    CommandCenterStatus = CommandCenterStatus.RoverProduction;
    var roverAssemblyStatus = _assemblyRoutine.Assemble(this);
    if (roverAssemblyStatus != null)
    {
        Resources["mineral"] -= roverCost;

        if (!exploring)
        {
            AssignResourceAndCommandCenterToTheRover(roverAssemblyStatus);
        }

        CommandCenterStatus = CommandCenterStatus.Idle;
    }

    return roverAssemblyStatus;
}
}
