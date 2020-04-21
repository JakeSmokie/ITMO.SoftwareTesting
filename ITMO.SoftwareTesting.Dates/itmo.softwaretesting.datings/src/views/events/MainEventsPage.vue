<template>
	<b-container>
		<b-nav tabs align="right" class="w-100 mx-3 mb-3">
			<b-nav-item :active="$route.fullPath === '/events'" to="/events">Все</b-nav-item>
			<b-nav-item :active="$route.fullPath === '/events/favorites'" to="/events/favorites">Любимые</b-nav-item>
		</b-nav>

		<router-view/>
		<create-date-modal/>
	</b-container>
</template>

<script>
	import {mapActions, mapGetters, mapState} from 'vuex';
	import CreateDateModal from '../../components/CreateDateModal';

	export default {
		name: 'MainEventsPage',
		components: {CreateDateModal},
		async created() {
			await this.loadLocations();
			await this.loadEventCategories();
			await this.listGroups();
			await this.loadDates();
			await this.loadEvents([]);
			await this.loadFavoriteEvents();
		},

		computed: {
			...mapGetters('groups', ['myGroups']),
			...mapGetters(['availableGroups']),
			...mapState('events', ['selectedEvent']),
			...mapState('dates', ['dates']),
		},

		methods: {
			...mapActions('events', ['loadFavoriteEvents']),
			...mapActions('groups', ['listGroups']),
			...mapActions('kudago', ['loadEventCategories', 'loadLocations']),
			...mapActions('events', ['loadEvents', 'selectEvent']),
			...mapActions('dates', ['createDate', 'loadDates']),
		},
	};
</script>

