using BeatThat.CollectionsExt;
using BeatThat.Controllers;
using BeatThat.GetComponentsExt;
using BeatThat.ItemManagers;
using BeatThat.Pools;
using BeatThat.Properties;
using BeatThat.TransformPathExt;
using UnityEngine;
using UnityEngine.UI;

namespace BeatThat.ScrollLists.SwipeNextEvents
{
    public class SwipeToSelectNext : Subcontroller<IHasSelect>
	{
		public ScrollListSwipeNextEvents m_events;
		public bool m_disableForceUpdate;
		public bool m_debug;

		override protected void BindSubcontroller()
		{
			RebindEvents ();
		}

		private void RebindEvents()
		{
			var events = GetComponentInChildren<ScrollListSwipeNextEvents> (true);
			if (events == null) {
				var scrollRect = GetComponentInChildren<ScrollRect> ();

				if (scrollRect == null) {
					#if UNITY_EDITOR || DEBUG_UNSTRIP
					if(m_debug) {
						Debug.LogWarning("[" + Time.frameCount + "][" + this.Path()
							+ "] cannot find an ScrollRect child (needed as attachment point for swipe events");
					}
					#endif
					return;
				}

				events = scrollRect.AddIfMissing<ScrollListSwipeNextEvents> ();
			}

			m_events = events;

			events.recenter.AddListener (this.OnRecenter);
			events.scrollPrev.AddListener (this.OnPrev);
			events.scrollNext.AddListener (this.OnNext);
		}

		private void OnNext()
		{
			#if UNITY_EDITOR || DEBUG_UNSTRIP
			if(m_debug) {
				Debug.Log("[" + Time.frameCount + "][" + this.Path() + "] OnNext");
			}
			#endif

			using (var items = ListPool<GameObject>.Get ()) {
				if (this.controller.GetItems<GameObject> (items) == 0) {
					return;
				}

				var curSelected = this.controller.selectedGameObject;

				if (curSelected == null) {
					Select(0);
					return;
				}

				var ix = items.FindIndex (x => x == curSelected);
				if (ix == -1) {
					Select (0);
					return;
				}

				Select (ix + 1 < items.Count ? ix + 1: items.Count - 1);
			}

		}

		private void Select(int index)
		{
			this.controller.Select (index, (m_disableForceUpdate) ? PropertyEventOptions.SendOnChange : PropertyEventOptions.Force);
		}

		private void OnPrev()
		{

			#if UNITY_EDITOR || DEBUG_UNSTRIP
			if(m_debug) {
				Debug.Log("[" + Time.frameCount + "][" + this.Path() + "] OnPrev");
			}
			#endif

			using (var items = ListPool<GameObject>.Get ()) {
				if (this.controller.GetItems<GameObject> (items) == 0) {
					return;
				}

				var curSelected = this.controller.selectedGameObject;

				if (curSelected == null) {
					Select (0);
					return;
				}

				var ix = items.FindIndex (x => x == curSelected);
				if (ix == -1) {
					Select (0);
					return;
				}

				Select (ix - 1 >= 0 ? ix - 1 : 0);
			}
		}

		private void OnRecenter()
		{

			#if UNITY_EDITOR || DEBUG_UNSTRIP
			if(m_debug) {
				Debug.Log("[" + Time.frameCount + "][" + this.Path() + "] OnRecenter");
			}
			#endif

			using (var items = ListPool<GameObject>.Get ()) {
				if (this.controller.GetItems<GameObject> (items) == 0) {
					return;
				}

				var curSelected = this.controller.selectedGameObject;

				if (curSelected == null) {
					Select (0);
					return;
				}

				var ix = items.FindIndex (x => x == curSelected);
				if (ix == -1) {
					Select (0);
					return;
				}

				Select (ix);
			}
		}

	}

}





