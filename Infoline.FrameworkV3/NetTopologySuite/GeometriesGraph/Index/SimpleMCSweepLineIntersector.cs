using System.Collections.Generic;

namespace NetTopologySuite.GeometriesGraph.Index
{
    /// <summary> 
    /// Finds all intersections in one or two sets of edges,
    /// using an x-axis sweepline algorithm in conjunction with Monotone Chains.
    /// While still O(n^2) in the worst case, this algorithm
    /// drastically improves the average-case time.
    /// The use of MonotoneChains as the items in the index
    /// seems to offer an improvement in performance over a sweep-line alone.
    /// </summary>
    public class SimpleMCSweepLineIntersector : EdgeSetIntersector
    {
        private readonly List<SweepLineEvent> _events = new List<SweepLineEvent>();

        // statistics information
        int _nOverlaps;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="edges"></param>
        /// <param name="si"></param>
        /// <param name="testAllSegments"></param>
        public override void ComputeIntersections(IList<Edge> edges, SegmentIntersector si, bool testAllSegments)
        {
            if (testAllSegments)
                 Add(edges, null);
            else Add(edges);
            ComputeIntersections(si);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="edges0"></param>
        /// <param name="edges1"></param>
        /// <param name="si"></param>
        public override void ComputeIntersections(IList<Edge> edges0, IList<Edge> edges1, SegmentIntersector si)
        {
            Add(edges0, edges0);
            Add(edges1, edges1);
            ComputeIntersections(si);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="edges"></param>
        private void Add(IEnumerable<Edge> edges)
        {
            foreach (Edge edge in edges)
            {
                // edge is its own group
                Add(edge, edge);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="edges"></param>
        /// <param name="edgeSet"></param>
        private void Add(IEnumerable<Edge> edges, object edgeSet)
        {
            foreach (Edge edge in edges)
            {
                Add(edge, edgeSet);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="edgeSet"></param>
        private void Add(Edge edge, object edgeSet)
        {
            MonotoneChainEdge mce = edge.MonotoneChainEdge;
            int[] startIndex = mce.StartIndexes;
            for (int i = 0; i < startIndex.Length - 1; i++) 
            {
                MonotoneChain mc = new MonotoneChain(mce, i);
                SweepLineEvent insertEvent = new SweepLineEvent(edgeSet, mce.GetMinX(i), mc);
                _events.Add(insertEvent);
                _events.Add(new SweepLineEvent(mce.GetMaxX(i), insertEvent));
            }
        }

        /// <summary>
        /// Because Delete Events have a link to their corresponding Insert event,
        /// it is possible to compute exactly the range of events which must be
        /// compared to a given Insert event object.
        /// </summary>
        private void PrepareEvents()
        {
            _events.Sort();
            // set DELETE event indexes
            for (int i = 0; i < _events.Count; i++ )
            {
                SweepLineEvent ev = _events[i];
                if (ev.IsDelete)
                    ev.InsertEvent.DeleteEventIndex = i;
            }            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="si"></param>
        private void ComputeIntersections(SegmentIntersector si)
        {
            _nOverlaps = 0;
            PrepareEvents();

            for (int i = 0; i < _events.Count; i++ )
            {
                SweepLineEvent ev = _events[i];
                if (ev.IsInsert)
                {
                    // Console.WriteLine("Processing event " + i);
                    ProcessOverlaps(i, ev.DeleteEventIndex, ev, si);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="ev0"></param>
        /// <param name="si"></param>
        private void ProcessOverlaps(int start, int end, SweepLineEvent ev0, SegmentIntersector si)
        {
            MonotoneChain mc0 = (MonotoneChain)ev0.Object;

            /*
            * Since we might need to test for self-intersections,
            * include current INSERT event object in list of event objects to test.
            * Last index can be skipped, because it must be a Delete event.
            */
            for (int i = start; i < end; i++ ) 
            {
                SweepLineEvent ev1 = _events[i]; 
                if (ev1.IsInsert) 
                {
                    MonotoneChain mc1 = (MonotoneChain)ev1.Object;
                    // don't compare edges in same group, if labels are present
                    if (!ev0.IsSameLabel(ev1)) 
                    {
                        mc0.ComputeIntersections(mc1, si);
                        _nOverlaps++;
                    }
                }
            }
        }
    }
}
