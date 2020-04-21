<template>
	<div v-if="eventDetails">
		<div class="d-flex justify-content-end">
			<b-button-group class="mb-3">
				<b-button
					variant="outline-primary"
					v-on:click="$bvModal.show('create-date-modal')"
					v-if="availableGroups.length > 0"
				>
					Создать встречу
				</b-button>
				<b-button variant="outline-success" v-if="!isFavorite" v-on:click="move(true)">Добавить в любимые</b-button>
				<b-button variant="outline-danger" v-else v-on:click="move(false)">Удалить из любимых</b-button>
			</b-button-group>
		</div>
		<b-card
			:title="eventDetails.title"
			:sub-title="eventDetails.tagline"
			:img-src="eventDetails.images[0].image"
			img-alt="Image"
			img-top
		>
			<h6 v-for="(date, i) in eventDetails.dates" :key="i">
				{{ new Date(date.start * 1000 + 10800000).toLocaleString('ru-RU') }}
				–
				{{ new Date(date.end * 1000 + 10800000).toLocaleString('ru-RU') }}
			</h6>

			<template v-if="eventDetails.place">
				<div v-if="eventDetails.place.title" class="py-2">
					<span>{{ eventDetails.place.title }}</span> <br>
					<span>{{ eventDetails.place.address }}, {{ eventDetails.place.subway }}</span>
				</div>
				<b-spinner variant="success" small v-else/>
			</template>

			<b-card-text class="text-justify" v-html="eventDetails.description"/>
			<b-card-text class="text-justify" v-html="eventDetails.body_text"/>
		</b-card>
	</div>
</template>
<script>
	import {mapActions, mapGetters, mapState} from 'vuex';

	export default {
		name: 'event-details',
		computed: {
			...mapState('events', ['favoriteEvents']),
			...mapState('dates', ['dates']),
			...mapGetters(['availableGroups']),
			...mapGetters('events', {
				eventDetails: 'selectedEventDetails',
			}),

			isFavorite() {
				return (this.favoriteEvents || []).includes((this.eventDetails || {id: -1}).id);
			},
		},

		methods: {
			...mapActions('events', ['addFavoriteEvent', 'removeFavoriteEvent']),
			...mapActions('dates', ['createDate']),

			async move(value) {
				if (value) {
					this.addFavoriteEvent(this.eventDetails.id);
				} else {
					this.removeFavoriteEvent(this.eventDetails.id);
				}
			},
		},
	};
</script>
