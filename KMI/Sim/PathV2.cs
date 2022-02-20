using System;
using System.Collections;
using System.Drawing;

namespace KMI.Sim {

    [Serializable] 
    public class PathV2 {

        public ArrayList nodes = new ArrayList();

        public ArrayList ToDistributedPoints() {
            ArrayList list = new ArrayList();
            for (int i = 1; i < this.nodes.Count; i++) {
                PointF distributedLocation = (PointF) ((NodeV2) this.nodes[i]).DistributedLocation;
                list.Add(distributedLocation);
            }
            return list;
        }

        public ArrayList ToPoints() {
            ArrayList list = new ArrayList();
            for (int i = 1; i < this.nodes.Count; i++) {
                PointF location = (PointF) ((NodeV2) this.nodes[i]).Location;
                list.Add(location);
            }
            return list;
        }
    }
}

